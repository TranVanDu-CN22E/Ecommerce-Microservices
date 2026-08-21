using OrderService.Application.DTOs;

namespace OrderService.Application.Interfaces.GRPC
{
    public interface IIdentityService
    {
        Task<GetUserDto?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
