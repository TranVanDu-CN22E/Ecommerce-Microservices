using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Common;
using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Interfaces;

namespace IdentityService.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler : ICommandHandler<LogoutCommand, Result>
    {
        private readonly IRefreshTokenRepository _refreshTokenRep;
        private readonly IUnitOfWork _unitOfWork;
        public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRep, IUnitOfWork unitOfWork)
        {
            _refreshTokenRep = refreshTokenRep;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userId = UserId.Create(Guid.Parse(request.UserId));
            await _refreshTokenRep.RevokeAllUserTokensAsync(userId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
