using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Abstractions.Services;
using IdentityService.Application.Common;
using IdentityService.Domain.Interfaces;

namespace IdentityService.Application.Features.User.Queries.GetUserById
{
    public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Result<GetUserByIdQueryResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        public GetUserByIdQueryHandler(IUserRepository userRepository, ICacheService cacheService)
        {
            _userRepository = userRepository;
            _cacheService = cacheService;
        }
        public async Task<Result<GetUserByIdQueryResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"GetUserByIdQuery:{request.UserId}";
            var cachedUser = await _cacheService.GetAsync<GetUserByIdQueryResponse>(cacheKey);
            if (cachedUser != null)
            {
                return Result<GetUserByIdQueryResponse>.Success(cachedUser);
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Result<GetUserByIdQueryResponse>.Failure(new List<Error> { new Error("User not found", $"No user found with ID {request.UserId}") });
            }
            var response = new GetUserByIdQueryResponse
            {
                UserId = user.Id.ToString(),
                Email = user.Email.ToString(),
                Phone = user.Phone.Value.ToString(),
                UserName = user.UserName.ToString(),
                IsLocked = user.IsLocked,
                IsBanned = user.IsBanned,
                CreatedAt = user.CreatedAt,
                LockedUntil = user.LockedUntil,
                BannedUntil = user.BannedUntil,
                UserRoles = user.UserRoles.Select(ur => new UserRoleItem
                {
                    Id = ur.Id.ToString(),
                    UserId = ur.UserId.Value.ToString(),
                    RoleId = ur.RoleId.Value.ToString()
                }).ToList()
            };
            await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10), cancellationToken);
            return Result<GetUserByIdQueryResponse>.Success(response);
        }
    }
}
