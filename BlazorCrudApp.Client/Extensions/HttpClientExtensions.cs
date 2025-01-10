using BlazorCrudApp.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

namespace BlazorCrudApp.Client.Extensions;

public static class HttpClientExtensions
{
	public static IServiceCollection AddClient<T>(this WebAssemblyHostBuilder builder) where T : class
	{
		builder.Services.AddRefitClient<T>().ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
#if !DEBUG
			.RemoveAllLoggers()
#endif
		;
		return builder.Services;
	}

	public static IServiceCollection AddRefitClients(this WebAssemblyHostBuilder builder)
	{
		// services
		builder.AddClient<IPersonalService>();
		builder.AddClient<IAuthService>();

		return builder.Services;
	}
}