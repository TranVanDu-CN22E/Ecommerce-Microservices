using IdentityService.Domain.Interfaces;

namespace IdentityService.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _uow;
        public UnitOfWork(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await _uow.SaveChangesAsync(ct);
        }
    }
}
