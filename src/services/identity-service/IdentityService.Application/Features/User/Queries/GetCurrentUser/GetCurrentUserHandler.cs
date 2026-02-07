using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Abstractions.Services;
using IdentityService.Application.Common;
using IdentityService.Application.DTOs;
using IdentityService.Domain.Interfaces;

namespace IdentityService.Application.Features.User.Queries.GetCurrentUser
{
    public class GetCurrentUserHandler : IQueryHandler<GetCurrentUserQuery, Result<UserResponse>>
    {
        private readonly IUserRepository _userRep;
        private readonly ICacheService _cacheService;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
        public GetCurrentUserHandler(IUserRepository userRep, ICacheService cacheService)
        {
            _userRep = userRep;
            _cacheService = cacheService;
        }

        public async Task<Result<UserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"user:{request.UserId}";
            var cachedUser = await _cacheService.GetAsync<UserResponse>(cacheKey, cancellationToken);
            if (cachedUser != null)
            {
                Console.WriteLine("User fetched from cache.");
                return Result<UserResponse>.Success(cachedUser);
            }

            var user = await _userRep.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Result<UserResponse>.Failure(new[] { AuthErrors.UserNotExist });
            }

            var userResponse = new UserResponse(
                user.Id.ToString(),
                user.Email.Value,
                user.Phone.Value,
                user.UserName.Value,
                user.IsLocked,
                user.FailedLoginAttempts,
                user.IsBanned,
                user.CreatedAt,
                user.LockedUntil,
                user.BannedUntil
            );

            await _cacheService.SetAsync(cacheKey, userResponse, CacheDuration, cancellationToken);

            return Result<UserResponse>.Success(userResponse);
        }
    }
}
