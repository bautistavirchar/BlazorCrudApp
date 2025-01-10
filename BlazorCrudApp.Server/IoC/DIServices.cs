using BlazorCrudApp.Server.Data;
using BlazorCrudApp.Server.Models;
using BlazorCrudApp.Server.Services;
using BlazorCrudApp.Shared.Models;
using BlazorCrudApp.Shared.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace BlazorCrudApp.Server.IoC;

public static class DIServices
{
	public static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddScoped<IPersonalService, PersonalService>();
		services.AddScoped<IAuthService, AuthService>();

		services.AddScoped<IValidator<PersonalModel>, PersonalModelValidator>();
		services.AddScoped<IValidator<LoginModel>, LoginModelValidator>();

		return services;
	}

	public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

		services.Configure<IdentityOptions>(options =>
		{
			// Password settings.
			options.Password.RequireDigit = false;
			options.Password.RequireLowercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequiredLength = 6;
			options.Password.RequiredUniqueChars = 0;

			// Lockout settings.
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
			options.Lockout.AllowedForNewUsers = false;

			// User settings.
			options.User.RequireUniqueEmail = true;
		});

		services.AddCascadingAuthenticationState();
		services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				// Cookie Settings
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromDays(30);
				options.LoginPath = "/login";
				options.LogoutPath = "/auth/logout";
				options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
				options.SlidingExpiration = true;
				options.Cookie.Name = $".{nameof(BlazorCrudApp)}.Identity.Application";
			});
	}
}
