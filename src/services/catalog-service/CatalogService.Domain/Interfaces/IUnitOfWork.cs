namespace CatalogService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        bool HasActiveTransaction { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync();
    }
}
