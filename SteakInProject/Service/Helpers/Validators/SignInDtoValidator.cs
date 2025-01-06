using System;
using FluentValidation;
using Service.Helpers.DTOs.Account;

namespace CompanyProject.Helpers.Validators
{
	public class SignInDtoValidator : AbstractValidator<SignInDto>
    {
        public SignInDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("email required").EmailAddress().WithMessage("format is wrong");
            RuleFor(x => x.Password).NotEmpty().WithMessage("password required");
        }
    }
}

