using FluentValidation;
using IdentityService.Application.Common;

namespace IdentityService.Application.Features.Auth.Commands.Register
{
    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator() {
            RuleFor(x => x.Email)
                .NotEmpty().WithErrorCode(AuthErrors.EmailAlreadyExists.Code).WithMessage(AuthErrors.EmailAlreadyExists.Message)
                .EmailAddress().WithErrorCode(AuthErrors.EmailInvalid.Code).WithMessage(AuthErrors.EmailInvalid.Message)
                .MaximumLength(255).WithErrorCode(AuthErrors.EmailMaxLength.Code).WithMessage(AuthErrors.EmailMaxLength.Message);
            RuleFor(x => x.Phone)
                .NotEmpty().WithErrorCode(AuthErrors.PhoneRequired.Code).WithMessage(AuthErrors.PhoneRequired.Message)
                .Matches(@"^[0-9]{9,15}$").WithErrorCode(AuthErrors.PhoneInvalid.Code).WithMessage(AuthErrors.PhoneInvalid.Message);
            RuleFor(x => x.UserName)
                .NotEmpty().WithErrorCode(AuthErrors.UsernameRequired.Code).WithMessage(AuthErrors.UsernameRequired.Message)
                .MinimumLength(3).WithErrorCode(AuthErrors.UsernameMinimum.Code).WithMessage(AuthErrors.UsernameMinimum.Message)
                .MaximumLength(50).WithErrorCode(AuthErrors.UsernameMaximum.Code).WithMessage(AuthErrors.UsernameMaximum.Message)
                .Matches(@"^[\p{L}0-9]+( [\p{L}0-9]+)*$").WithErrorCode(AuthErrors.UsernameInvalid.Code).WithMessage(AuthErrors.UsernameInvalid.Message);
            RuleFor(x => x.Password)
                .NotEmpty().WithErrorCode(AuthErrors.PasswordRequired.Code).WithMessage(AuthErrors.PasswordRequired.Message)
                .MinimumLength(8).WithErrorCode(AuthErrors.PasswordMinimum.Code).WithMessage(AuthErrors.PasswordMinimum.Message)
                .MaximumLength(128).WithErrorCode(AuthErrors.PasswordMaximum.Code).WithMessage(AuthErrors.PasswordMaximum.Message)
                .Matches(@"[A-Z]").WithErrorCode(AuthErrors.PasswordUppercase.Code).WithMessage(AuthErrors.PasswordUppercase.Message)
                .Matches(@"[a-z]").WithErrorCode(AuthErrors.PasswordLowercase.Code).WithMessage(AuthErrors.PasswordLowercase.Message)
                .Matches(@"[0-9]").WithErrorCode(AuthErrors.PasswordDigit.Code).WithMessage(AuthErrors.PasswordDigit.Message)
                .Matches(@"[!@#$%&*?]").WithErrorCode(AuthErrors.PasswordSpecialChar.Code).WithMessage(AuthErrors.PasswordSpecialChar.Message);
        }
    }
}
