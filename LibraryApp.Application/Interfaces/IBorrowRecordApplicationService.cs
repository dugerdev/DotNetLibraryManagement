using LibraryApp.Application.DTOs.BorrowRecords;

namespace LibraryApp.Application.Interfaces;

/// <summary>
/// Ödünç verme kaydı application service interface'i
/// Bu interface, ödünç verme kayıtları ile ilgili tüm application işlemlerini tanımlar
/// </summary>
public interface IBorrowRecordApplicationService
{
    Task<Guid> CreateBorrowRecordAsync(CreateBorrowRecordDto dto, CancellationToken cancellationToken = default);
    Task<BorrowRecordDto?> GetBorrowRecordByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BorrowRecordDto>> GetAllBorrowRecordsAsync(CancellationToken cancellationToken = default);
    Task<bool> ReturnBookAsync(Guid borrowRecordId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BorrowRecordDto>> GetBorrowRecordsByMemberAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BorrowRecordDto>> GetBorrowRecordsByBookAsync(Guid bookId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BorrowRecordDto>> GetActiveBorrowRecordsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BorrowRecordDto>> GetOverdueBorrowRecordsAsync(CancellationToken cancellationToken = default);
    Task<decimal> CalculateFineAsync(Guid borrowRecordId, CancellationToken cancellationToken = default);
}
