using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace ServISWebApp.Auth
{
    /// <summary>
    /// Provides the custom implementation of the authentication state for the application.
    /// </summary>
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly ProtectedLocalStorage localStorage;
		private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuthenticationStateProvider"/> class.
        /// </summary>
        /// <param name="localStorage">The protected local storage used for storing user authentication information.</param>
        public CustomAuthenticationStateProvider(ProtectedLocalStorage localStorage)
		{
			this.localStorage = localStorage;
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
					return await Task.FromResult(new AuthenticationState(anonymous));
				}

				var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
				{
					new Claim(ClaimTypes.PrimarySid, userLocalStorage.User.Id.ToString()),

					new Claim(ClaimTypes.Role, userLocalStorage.User.Role)
				}, "CustomAuth"));

				return await Task.FromResult(new AuthenticationState(claimsPrincipal));
			}
			catch
			{
				return await Task.FromResult(new AuthenticationState(anonymous));
			}
		}

        /// <summary>
        /// Updates the authentication state based on the provided user local storage information.
        /// </summary>
        /// <param name="userLocalStorage">The user local storage information to update the authentication state with. 
        /// Use <c>null</c> to indicate a logout operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateAuthenticationState(UserLocalStorage? userLocalStorage)
		{
			ClaimsPrincipal claimsPrincipal;

			if (userLocalStorage != null)
			{// login
				await localStorage.SetAsync(nameof(UserLocalStorage), userLocalStorage);
				claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
				{
					new Claim(ClaimTypes.PrimarySid, userLocalStorage.User.Id.ToString()),

					new Claim(ClaimTypes.Role, userLocalStorage.User.Role)
				}, "CustomAuth"));
			}
			else
			{// logout
				await localStorage.DeleteAsync(nameof(UserLocalStorage));
				claimsPrincipal = anonymous;
			}

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
		}
	}
}
