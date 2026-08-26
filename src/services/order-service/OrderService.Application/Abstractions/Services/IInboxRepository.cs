using OrderService.Application.DTOs;

namespace OrderService.Application.Abstractions.Services
{
    public interface IInboxRepository
    {
        /// <summary>
        /// Kiểm tra message đã được xử lý thành công chưa (idempotency check)
        /// </summary>
        Task<bool> IsProcessedAsync(Guid messageId, CancellationToken ct = default);

        /// <summary>
        /// Thêm inbox message vào DB (trong transaction, chưa commit)
        /// Unique constraint trên Id sẽ throw nếu duplicate → auto rollback
        /// </summary>
        Task AddAsync(InboxMessageDto message, CancellationToken ct = default);

        /// <summary>
        /// Đánh dấu message đã xử lý thành công
        /// </summary>
        Task MarkAsProcessedAsync(Guid messageId, CancellationToken ct = default);

        /// <summary>
        /// Lưu lỗi khi xử lý thất bại (không throw, best-effort)
        /// </summary>
        Task MarkAsFailedAsync(Guid messageId, string error, CancellationToken ct = default);
    }
}
