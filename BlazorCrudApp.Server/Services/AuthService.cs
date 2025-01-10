using BlazorCrudApp.Shared;
using BlazorCrudApp.Server.Data;
using BlazorCrudApp.Server.Extensions;
using BlazorCrudApp.Server.Models;
using BlazorCrudApp.Shared.Models;
using BlazorCrudApp.Shared.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorCrudApp.Server.Services;
public interface IAuthService
{
	Task<ApiResponse> AuthenticateAsync(LoginModel loginModel);
}

public class AuthService : DbContextConnection, IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly SignInManager<ApplicationUser> _signInManager;
	public AuthService(IDbContextFactory<ApplicationDbContext> dbContextFactory, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : base(dbContextFactory)
	{
		_userManager = userManager;
		_signInManager = signInManager;
	}

	public async Task<ApiResponse> AuthenticateAsync(LoginModel loginModel)
	{
		var user = await _userManager.FindByEmailAsync(loginModel.Email);

		var authMessage = "Authentication failed";
		if (user is null)
		{
			user = await _userManager.FindByNameAsync(loginModel.Email);
			if (user is null)
				return ApiResponse.ErrorResponse(authMessage);
		}

		if (await _userManager.IsInRoleAsync(user, "Client"))
			return ApiResponse.ErrorResponse(authMessage);

		// check password
		var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
		if (!checkPassword.Succeeded)
			return ApiResponse.ErrorResponse(authMessage);

		var name = "Administrator";

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.GivenName, name)
		};

		var authProperties = new AuthenticationProperties
		{
			AllowRefresh = true,
			IsPersistent = false,
			IssuedUtc = DateTime.UtcNow,
			ExpiresUtc = DateTime.UtcNow.AddHours(10),
		};
#if DEBUG
		authProperties.IsPersistent = true;

#endif
		await _signInManager.SignInWithClaimsAsync(user, authProperties, claims);

		var roles = await _userManager.GetRolesAsync(user);
		return ApiResponse.SuccessResponse(new UserProfileViewModel
		{
			Name = name,
			Email = loginModel.Email,
			Role = roles![0]
		});
	}
}

