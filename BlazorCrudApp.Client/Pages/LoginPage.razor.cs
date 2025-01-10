using BlazorCrudApp.Client.Services;
using BlazorCrudApp.Shared.Models;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;

namespace BlazorCrudApp.Client.Pages;

public partial class LoginPage
{
	[Inject]
	private IAuthService AuthService { get; set; } = default!;
	[Inject]
	private NavigationManager Navigation { get; set; } = default!;
	private LoginModel Model { get; set; } = new();
	private string? Message { get; set; }
	public bool IsBusy { get; set; }

	FluentValidationValidator FluentValidationValidator { get; set; } = default!;
	async Task SubmitAsync()
	{
		IsBusy = true;
		Message = string.Empty;
		if (await FluentValidationValidator.ValidateAsync())
		{
			var response = await AuthService.LoginAsync(Model);
			if (response.Success)
			{
				Navigation.NavigateTo("/", true, true);
				return;
			}
			Message = response.ErrorMessage;
		}

		IsBusy = false;
	}
}
