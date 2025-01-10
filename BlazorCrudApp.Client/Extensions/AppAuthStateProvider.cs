using BlazorCrudApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorCrudApp.Client.Extensions;

public static class AppAuthStateProviderExtension
{
	public static ClaimsPrincipal ToClaimsPrincipal(this UserProfileViewModel userProfileViewModel)
	{
		var identity = new ClaimsIdentity(new[]{
				new Claim(ClaimTypes.Role, userProfileViewModel!.Role),
				new Claim(ClaimTypes.Email, userProfileViewModel!.Email),
				new Claim(ClaimTypes.GivenName, userProfileViewModel!.Name),
			}, "Cookies");
		return new ClaimsPrincipal(identity);
	}
}

public class AppAuthStateProvider : AuthenticationStateProvider
{
	private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
	public override async Task<AuthenticationState> GetAuthenticationStateAsync() =>
		await Task.FromResult(new AuthenticationState(claimsPrincipal));

	public void SetUserInfo(UserProfileViewModel userProfileViewModel)
	{
		if (userProfileViewModel is not null)
		{
			claimsPrincipal = userProfileViewModel.ToClaimsPrincipal();
		}
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public void Logout() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
}
