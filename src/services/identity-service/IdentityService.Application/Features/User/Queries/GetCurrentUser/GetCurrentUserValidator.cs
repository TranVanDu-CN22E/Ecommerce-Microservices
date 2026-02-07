using FluentValidation;
using IdentityService.Application.Common;

namespace IdentityService.Application.Features.User.Queries.GetCurrentUser
{
    public class GetCurrentUserValidator : AbstractValidator<GetCurrentUserQuery>
    {
        public GetCurrentUserValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull().WithErrorCode(AuthErrors.UserIdRequired.Code).WithMessage(AuthErrors.UserIdRequired.Message);
        }
    }
}
