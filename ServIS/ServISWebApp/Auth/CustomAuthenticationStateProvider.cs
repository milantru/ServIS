using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Text.Json;

namespace ServISWebApp.Auth
{
	public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly ProtectedSessionStorage sessionStorage;
		private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());

		public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
		{
			this.sessionStorage = sessionStorage;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			try
			{
				var userSessionStorageResult = await sessionStorage.GetAsync<UserSessionStorage>(nameof(UserSessionStorage));
				var userSessionStorage = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
				if (userSessionStorage == null)
				{
					return await Task.FromResult(new AuthenticationState(anonymous));
				}

				var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
				{
					new Claim(ClaimTypes.PrimarySid, userSessionStorage.User.Id.ToString()),

					new Claim(ClaimTypes.Role, userSessionStorage.User.Role)
				}, "CustomAuth"));

				return await Task.FromResult(new AuthenticationState(claimsPrincipal));
			}
			catch
			{
				return await Task.FromResult(new AuthenticationState(anonymous));
			}
		}

		public async Task UpdateAuthenticationState(UserSessionStorage? userSessionStorage)
		{
			ClaimsPrincipal claimsPrincipal;

			if (userSessionStorage != null)
			{// login
				await sessionStorage.SetAsync(nameof(UserSessionStorage), userSessionStorage);
				claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
				{
					new Claim(ClaimTypes.PrimarySid, userSessionStorage.User.Id.ToString()),

					new Claim(ClaimTypes.Role, userSessionStorage.User.Role)
				}, "CustomAuth"));
			}
			else
			{// logout
				await sessionStorage.DeleteAsync(nameof(UserSessionStorage));
				claimsPrincipal = anonymous;
			}

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
		}
	}
}
