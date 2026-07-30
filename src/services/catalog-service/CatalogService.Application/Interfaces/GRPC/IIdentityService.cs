using CatalogService.Application.DTOs;
using IdentityShared.Models;
namespace CatalogService.Application.Interfaces.GRPC
{
    public interface IIdentityService
    {
        Task<GetUserDto?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
