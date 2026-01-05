using FluentValidation;
using IdentityService.Application.Common;

namespace IdentityService.Application.Features.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(AuthErrors.EmailRequired.Code).WithMessage(AuthErrors.EmailRequired.Message)
            .EmailAddress().WithErrorCode(AuthErrors.EmailInvalid.Code).WithMessage(AuthErrors.EmailInvalid.Message);

            RuleFor(x => x.Password)
                .NotEmpty().WithErrorCode(AuthErrors.PasswordRequired.Code).WithMessage(AuthErrors.PasswordRequired.Message)
                .MaximumLength(128).WithErrorCode(AuthErrors.PasswordMaximum.Code).WithMessage(AuthErrors.PasswordMaximum.Message);
        }
    }
}
