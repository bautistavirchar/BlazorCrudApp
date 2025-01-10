using BlazorCrudApp.Shared.Models;
using FluentValidation;

namespace BlazorCrudApp.Shared.Validators;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
	public LoginModelValidator()
	{
		RuleFor(l => l.Email).NotNull().WithMessage(Global.REQUIRED_STRING);
		RuleFor(l => l.Password).NotNull().WithMessage(Global.REQUIRED_STRING);
	}
}
