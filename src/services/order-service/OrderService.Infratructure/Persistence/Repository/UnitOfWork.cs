using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Interface;

namespace OrderService.Infratructure.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext _context;
        private IDbContextTransaction? _transaction;
        public UnitOfWork(OrderDbContext context)
        {
            _context = context;
        
        }
        public bool HasActiveTransaction => _transaction != null;

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _transaction = await _context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No active transaction to commit.");
            }

            try
            {
                await _context.SaveChangesAsync(ct);
                await _transaction.CommitAsync(ct);
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken ct = default)
        {
            // Nếu đã có transaction rồi thì không tạo mới
            if (_transaction != null)
            {
                return await action();
            }

            // Tạo execution strategy để retry khi lỗi connection
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(ct);

                try
                {
                    var result = await action();
                    await transaction.CommitAsync(ct);
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            });
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null) return;
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
