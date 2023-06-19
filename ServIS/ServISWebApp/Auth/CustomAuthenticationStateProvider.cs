using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ServISData.Interfaces;
using ServISData.Models;
using System.Security.Claims;

namespace ServISWebApp.Auth
{
    /// <summary>
    /// Provides the custom implementation of the authentication state for the application.
    /// </summary>
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly ProtectedLocalStorage localStorage;
		private readonly IServISApi api;
		private readonly ILogger<CustomAuthenticationStateProvider> logger;
		private readonly ClaimsPrincipal anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomAuthenticationStateProvider"/> class.
		/// </summary>
		/// <param name="localStorage">The protected local storage used for storing user authentication information.</param>
		public CustomAuthenticationStateProvider(
			ProtectedLocalStorage localStorage, 
			IServISApi api, 
			ILogger<CustomAuthenticationStateProvider> logger
		)
		{
			this.localStorage = localStorage;
			this.api = api;
			this.logger = logger;
		}

        /// <summary>
        /// Retrieves the authentication state asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing the authentication state.</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			try
			{
				var userLocalStorageResult = await localStorage.GetAsync<UserLocalStorage>(nameof(UserLocalStorage));
				var userLocalStorage = userLocalStorageResult.Success ? userLocalStorageResult.Value : null;
				if (userLocalStorage == null)
				{
					return await Task.FromResult(new AuthenticationState(anonymousUser));
				}
				var claimsPrincipal = GetClaimsPrincipal(userLocalStorage);

				return await Task.FromResult(new AuthenticationState(claimsPrincipal));
			}
			catch
			{
				return await Task.FromResult(new AuthenticationState(anonymousUser));
			}
		}

		/// <summary>
		/// Logs in the user and updates the authentication state with the provided user information.
		/// </summary>
		/// <param name="user">The user object representing the authenticated user.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public async Task LogInAsync(User user)
		{
			var userLocalStorage = new UserLocalStorage(user);

			await UpdateAuthenticationStateAsync(userLocalStorage);
		}

		/// <summary>
		/// Logs out the currently authenticated user and updates the authentication state accordingly.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public async Task LogoutAsync()
		{
			await UpdateAuthenticationStateAsync(null);
		}

		/// <summary>
		/// Retrieves the logged-in user asynchronously based on the provided authentication state.
		/// </summary>
		/// <param name="authState">The current authentication state.</param>
		/// <returns>A task that represents the asynchronous operation, containing the logged-in user object if found; otherwise, returns null.</returns>
		public async Task<User?> GetLoggedInUserAsync(AuthenticationState authState)
		{
			int userId = 0;
			User? user;

			try
			{
				userId = int.Parse(
					authState.User.Claims.First(c => c.Type == ClaimTypes.PrimarySid).Value
				);

				user = await api.GetUserAsync(userId);
			}
			catch (Exception ex)
			{
				// This is only a warning and not an error because it is triggered also e.g. when the user is in the profile section and logs out.
				logger.LogWarning(ex, $"Failed to load user with id '{userId}'.");

				user = null;
			}

			return user;
		}

		/// <summary>
		/// Checks if the currently logged-in user, based on the provided authentication state, is authenticated as an administrator.
		/// </summary>
		/// <param name="authState">The current authentication state.</param>
		/// <returns>
		/// A task that represents the asynchronous operation, returning <c>true</c> if the user is authenticated as an administrator,
		/// and <c>false</c> otherwise.
		/// </returns>
		public async Task<bool> CheckIfLoggedInAsAdminAsync(AuthenticationState authState)
		{
			var user = await GetLoggedInUserAsync(authState);

			return user?.Role == "Administrator";
		}

		private static ClaimsPrincipal GetClaimsPrincipal(UserLocalStorage userLocalStorage)
		{
			var claimsIdentity = new ClaimsIdentity(
				claims: new List<Claim>
				{
					new Claim(ClaimTypes.PrimarySid, userLocalStorage.User.Id.ToString()),
					new Claim(ClaimTypes.Role, userLocalStorage.User.Role)
				},
				authenticationType: "CustomAuth"
			);

			return new ClaimsPrincipal(claimsIdentity);
		}

		/// <summary>
		/// Updates the authentication state based on the provided user local storage information.
		/// </summary>
		/// <param name="userLocalStorage">The user local storage information to update the authentication state with. 
		/// Use <c>null</c> to indicate a logout operation.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		private async Task UpdateAuthenticationStateAsync(UserLocalStorage? userLocalStorage)
		{
			ClaimsPrincipal claimsPrincipal;

			if (userLocalStorage is not null)
			{// login
				await localStorage.SetAsync(nameof(UserLocalStorage), userLocalStorage);
				claimsPrincipal = GetClaimsPrincipal(userLocalStorage);
			}
			else
			{// logout
				await localStorage.DeleteAsync(nameof(UserLocalStorage));
				claimsPrincipal = anonymousUser;
			}

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
		}
	}
}
