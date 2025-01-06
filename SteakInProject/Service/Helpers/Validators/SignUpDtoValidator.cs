using System;
using FluentValidation;
using Service.Helpers.DTOs.Account;

namespace CompanyProject.Helpers.Validators
{
	public class SignUpDtoValidator:AbstractValidator<SignUpDto>
	{
		public SignUpDtoValidator()
		{
			RuleFor(x => x.FullName).NotNull().WithMessage("fullname required");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("username required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("email required").EmailAddress().WithMessage("format is wrong");
            RuleFor(x => x.Password).NotEmpty().WithMessage("password required");
        }
	}
}

