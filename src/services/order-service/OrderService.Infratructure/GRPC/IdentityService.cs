using IdentityShared.Protos;
using OrderService.Application.DTOs;
using UserRoleDto = OrderService.Application.DTOs.UserRoleDto;
namespace OrderService.Infratructure.GRPC
{
    public sealed class IdentityService : OrderService.Application.Interfaces.GRPC.IIdentityService
    {
        private readonly UserGrpcService.UserGrpcServiceClient _userClient;

        public IdentityService(UserGrpcService.UserGrpcServiceClient userClient)
        {
            _userClient = userClient;
        }

        public async Task<GetUserDto?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _userClient.GetUserByIdAsync(new GetUserByIdRequest { UserId = userId }, cancellationToken: cancellationToken);
                if (response != null)
                {
                    var result = new GetUserDto
                    {
                        UserId = response.User.UserId,
                        Email = response.User.Email,
                        Phone = response.User.Phone,
                        UserName = response.User.UserName,
                        IsLocked = response.User.IsLocked,
                        IsBanned = response.User.IsBanned,
                        CreatedAt = response.User.CreatedAt.ToDateTime(),
                        LockedUntil = response.User.LockedUntil?.ToDateTime(),
                        BannedUntil = response.User.BannedUntil?.ToDateTime(),
                        UserRoles = response.User.UserRoles.Select(role => new UserRoleDto
                        {
                            Id = role.Id,
                            RoleId = role.RoleId,
                            UserId = role.UserId,

                        }).ToList()
                    };
                    return result;
                }
                else return null;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use your preferred logging framework)
                Console.WriteLine($"Error in GetUserByIdAsync: {ex.Message}");
                return null;
            }
        }
    }
}
