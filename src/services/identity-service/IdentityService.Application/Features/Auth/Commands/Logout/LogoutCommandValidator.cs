using FluentValidation;
using IdentityService.Application.Common;
using Microsoft.AspNetCore.Rewrite;

namespace IdentityService.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandValidator :AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator() 
        {
            RuleFor(x => x.UserId)
                .NotNull().WithErrorCode(AuthErrors.UserIdRequired.Code).WithMessage(AuthErrors.UserIdRequired.Message);
            
        }
    }
}
