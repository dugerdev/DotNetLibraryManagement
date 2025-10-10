using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;

namespace LibraryApp.Domain.Common;

/// <summary>
/// BorrowRecord entity'sine özel repository interface'i
/// Ödünç verme kayıtlarıyla ilgili domain-specific işlemleri tanımlar
/// </summary>
public interface IBorrowRecordRepository : IRepository<BorrowRecord>
{
    // BorrowRecord-specific queries - Ödünç kayıtlarına özel sorgular
    Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByMemberAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByBookAsync(Guid bookId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BorrowRecord>> GetActiveBorrowRecordsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByStatusAsync(BorrowStatus status, CancellationToken cancellationToken = default);
    Task<BorrowRecord?> GetActiveBorrowRecordAsync(Guid memberId, Guid bookId, CancellationToken cancellationToken = default);

    // Statistics - İstatistikler
    Task<int> GetActiveBorrowCountAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<int> GetTotalBorrowCountAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalFineAmountAsync(Guid memberId, CancellationToken cancellationToken = default);

    // Business operations - İş kuralları
    Task<bool> CanMemberBorrowAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<bool> HasOverdueBooksAsync(Guid memberId, CancellationToken cancellationToken = default);
}