using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Abstractions.Services;
using IdentityService.Application.Common;
using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Aggregates.RefreshTokenAggregate;
using IdentityService.Domain.Interfaces;

namespace IdentityService.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, ct);
            if(user is null) return Result<LoginResponse>.Failure(new[] { AuthErrors.LoginFailed });
            if(!user.CanLogin()) return Result<LoginResponse>.Failure(new[] { new Error("User.LoginLocked", $"Account is locked until {user.LockedUntil:yyyy-MM-dd HH:mm:ss}") });

            if(!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash.Value))
            {
                user.IncreaseFailedLogin();
                await _unitOfWork.SaveChangesAsync(ct);
                return Result<LoginResponse>.Failure(new[] {AuthErrors.LoginFailed });
            }

            user.ResetFailedLogin();
            var userId = UserId.Create(user.Id);
            var roles = await _userRoleRepository.GetRoleIdsByUserAsync(userId, ct);
            var accessToken = _jwtTokenService.GenerateAccessToken(user, roles);
            var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();
            
            var refreshToken = Domain.Aggregates.RefreshTokenAggregate.RefreshToken.Create(
                UserId.Create(user.Id),
                refreshTokenValue,
                DateTime.UtcNow.AddDays(7)
            );
            await _refreshTokenRepository.RevokeAllUserTokensAsync(userId, ct);
            await _refreshTokenRepository.AddAsync(refreshToken, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<LoginResponse>.Success(new LoginResponse(
                accessToken,
                refreshTokenValue,
                DateTime.UtcNow.AddMinutes(10)
            ));
        }
    }
}
