using Google.Protobuf.WellKnownTypes;
using IdentityService.Application.Features.User.Queries.GetUserById;
using IdentityShared.Models;
using IdentityShared.Protos;

namespace IdentityShared.Extensions
{
    public static class GrpcMappingExtensions
    {
        public static UserDto ToProto(this UserGrpcModel model)
        {
            if (model == null) return null;

            var dto = new UserDto
            {
                UserId = model.UserId,
                Email = model.Email,
                Phone = model.Phone,
                UserName = model.UserName,
                IsLocked = model.IsLocked,
                IsBanned = model.IsBanned,

                // Xử lý DateTime -> Timestamp
                CreatedAt = Timestamp.FromDateTime(model.CreatedAt.ToUniversalTime()),
                LockedUntil = model.LockedUntil.HasValue
                    ? Timestamp.FromDateTime(model.LockedUntil.Value.ToUniversalTime())
                    : null,
                BannedUntil = model.BannedUntil.HasValue
                    ? Timestamp.FromDateTime(model.BannedUntil.Value.ToUniversalTime())
                    : null
            };

            // Mapping nested list
            foreach (var role in model.UserRoles)
            {
                dto.UserRoles.Add(new UserRoleDto
                {
                    Id = role.Id,
                    UserId = role.UserId,
                    RoleId = role.RoleId
                });
            }
            return dto;
        }

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
