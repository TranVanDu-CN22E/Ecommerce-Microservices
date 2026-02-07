using FluentValidation;
using IdentityService.Application.Common;

namespace IdentityService.Application.Features.Auth.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithErrorCode(AuthErrors.RefreshTokenRequired.Code).WithMessage(AuthErrors.RefreshTokenRequired.Message);
        }
    }
}
