using IdentityService.Application.Features.User.Queries.GetUserById;
using IdentityShared.Models;

namespace IdentityService.Application.Mapping
{
    public static class GrpcMapping
    {
        public static UserGrpcModel ToSharedModel(this GetUserByIdQueryResponse response)
        {
            if (response == null) return null;

            return new UserGrpcModel
            {
                UserId = response.UserId,
                Email = response.Email,
                Phone = response.Phone,
                UserName = response.UserName,
                IsLocked = response.IsLocked,
                IsBanned = response.IsBanned,
                CreatedAt = response.CreatedAt,
                LockedUntil = response.LockedUntil,
                BannedUntil = response.BannedUntil,
                UserRoles = response.UserRoles.Select(r => new UserRoleGrpcModel
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    RoleId = r.RoleId
                }).ToList()
            };
        }
    }
}
