using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Abstractions.Services;
using IdentityService.Application.Common;
using IdentityService.Domain.Aggregates.RefreshTokenAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Interfaces;

namespace IdentityService.Application.Features.Auth.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRep;
        private readonly IUserRepository _userRep;
        private readonly IUserRoleRepository _roleRep;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRep,
            IUserRepository userRep,
            IUserRoleRepository roleRep,
            IUnitOfWork unitOfWork,
            IJwtTokenService jwtTokenService)
        {
            _refreshTokenRep = refreshTokenRep;
            _userRep = userRep;
            _roleRep = roleRep;
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRep.GetByTokenAsync(request.RefreshToken, cancellationToken);
            if (refreshToken is null) return Result<RefreshTokenResponse>.Failure(new[] { AuthErrors.RefreshTokenNotExist });
            if (refreshToken.IsExpired() == true || refreshToken.IsRevoked == true) return Result<RefreshTokenResponse>.Failure([AuthErrors.RefreshTokenExpired]);

            var user = await _userRep.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null || user.IsLocked == true) return Result<RefreshTokenResponse>.Failure([AuthErrors.UserNotExist]);

            var roles = await _roleRep.GetRoleIdsByUserAsync(request.UserId, cancellationToken);
            var accessToken = _jwtTokenService.GenerateAccessToken(user, roles);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            await _refreshTokenRep.RevokeAllUserTokensAsync(request.UserId, cancellationToken);
            await _refreshTokenRep.AddAsync(request.UserId, newRefreshToken, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<RefreshTokenResponse>.Success( new RefreshTokenResponse(
                accessToken,
                newRefreshToken,
                DateTime.UtcNow.AddMinutes(10)
                )
            );
        }
    }
}
