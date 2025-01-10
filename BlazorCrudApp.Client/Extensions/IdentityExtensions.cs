using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorCrudApp.Client.Extensions;

public static class IdentityExtensions
{
	public static IServiceCollection AddidentityServices(this IServiceCollection services)
	{
		services.AddAuthorizationCore();
		services.AddCascadingAuthenticationState();
		services.AddScoped<AppAuthStateProvider>();
		services.AddScoped<AuthenticationStateProvider>(auth => auth.GetRequiredService<AppAuthStateProvider>());
		return services;
	}
}
