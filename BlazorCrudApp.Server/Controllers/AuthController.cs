using BlazorCrudApp.Server.Models;
using BlazorCrudApp.Server.Services;
using BlazorCrudApp.Shared.Models;
using BlazorCrudApp.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorCrudApp.Server.Controllers;

public class AuthController : IControllerBase<IAuthService>
{
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<ApplicationUser> _userManager;
	public AuthController(IAuthService service, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) : base(service)
	{
		_roleManager = roleManager;
		_userManager = userManager;
	}

	[HttpPost("login"), AllowAnonymous]
	public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel)
	{
		var response = await _service.AuthenticateAsync(loginModel);
		return Ok(response);
	}

	[HttpGet(nameof(Validate))]
	public IActionResult Validate()
	{
		if (!User.Identity!.IsAuthenticated) return Unauthorized();

		return Ok();
	}

	[HttpGet(nameof(GetInfo))]
	public IActionResult GetInfo()
	{
		var name = User.FindFirstValue(ClaimTypes.GivenName);
		var email = User.FindFirstValue(ClaimTypes.Email);
		var role = User.FindFirstValue(ClaimTypes.Role);

		return Ok(new UserProfileViewModel
		{
			Name = name!,
			Email = email!,
			Role = role!
		});
	}

	[HttpPost("seed"), AllowAnonymous]
	public async Task<IActionResult> SeedAsync()
	{
		await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
		await _roleManager.CreateAsync(new IdentityRole { Name = "User" });

		var adminAccount = new ApplicationUser
		{
			UserName = "admin",
			Email = "bautistavirchar@gmail.com",
			EmailConfirmed = true,
			PhoneNumber = "09278730477",
			PhoneNumberConfirmed = true
		};
		var identityResult = await _userManager.CreateAsync(adminAccount, "Hello@321");
		if (identityResult.Succeeded)
		{
			await _userManager.AddToRoleAsync(adminAccount, "Admin");
			return Ok();
		}
		return BadRequest();
	}
}
