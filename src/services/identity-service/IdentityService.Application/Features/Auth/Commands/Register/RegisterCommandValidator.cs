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
                .NotEmpty().WithMessage("Phone is required")
                .Matches(@"^[0-9]{10-11}$").WithMessage("Invalid phone format");
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters")
                .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers and underscores");
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
