namespace OrderService.Domain.Interface
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        /// Commit transaction
        Task CommitTransactionAsync(CancellationToken ct = default);
        /// Rollback transaction
        Task RollbackTransactionAsync();
        /// Kiểm tra có transaction đang chạy không
        bool HasActiveTransaction { get; }
        /// Thực thi action trong transaction với retry strategy
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken ct = default);
    }
}
