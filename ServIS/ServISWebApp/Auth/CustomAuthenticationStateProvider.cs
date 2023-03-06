using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Text.Json;

namespace ServISWebApp.Auth
{
	public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly ProtectedLocalStorage localStorage;
		private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());

		public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
			ProtectedLocalStorage localStorage,
		{
			this.localStorage = localStorage;
		}

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
