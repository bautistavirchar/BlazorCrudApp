using BlazorCrudApp.Shared.Models;
using BlazorCrudApp.Shared.ViewModels;
using Refit;

namespace BlazorCrudApp.Client.Services;

public interface IAuthService
{
	[Post("/api/auth/login")]
	Task<Shared.ApiResponse> LoginAsync(LoginModel loginModel);

	[Get("/api/auth/Validate")]
	Task<IApiResponse> ValidateAsync();

	[Get("/api/auth/GetInfo")]
	Task<UserProfileViewModel> GetInfoAsync();
}
