using BlazorCrudApp.Shared.Models;
using FluentValidation;

namespace BlazorCrudApp.Shared.Validators;

public class PersonalModelValidator : AbstractValidator<PersonalModel>
{
	public PersonalModelValidator()
	{
		RuleFor(p => p.FirstName).NotNull().WithMessage(Global.REQUIRED_STRING);
		RuleFor(p => p.LastName).NotNull().WithMessage(Global.REQUIRED_STRING);
		RuleFor(p => p.DateOfBirth).NotNull().WithMessage(Global.REQUIRED_STRING);
	}
}
