using LibraryApp.Application.DTOs.BorrowRecords;
using LibraryApp.Application.Interfaces;
using LibraryApp.Application.Mappers;
using LibraryApp.Domain.Common;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Application.Services;

/// <summary>
/// Ödünç verme kaydı application service implementasyonu
/// </summary>
public class BorrowRecordApplicationService : IBorrowRecordApplicationService
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IBorrowingService _borrowingService;
    private readonly IUnitOfWork _unitOfWork;

    public BorrowRecordApplicationService(
        IBorrowRecordRepository borrowRecordRepository,
        IBorrowingService borrowingService,
        IUnitOfWork unitOfWork)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _borrowingService = borrowingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateBorrowRecordAsync(CreateBorrowRecordDto dto, CancellationToken cancellationToken = default)
    {
        // Domain service kullanarak ödünç verme işlemini yap
        var borrowRecord = await _borrowingService.BorrowBookAsync(dto.MemberId, dto.BookId, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return borrowRecord.Id;
    }

    public async Task<BorrowRecordDto?> GetBorrowRecordByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var borrowRecord = await _borrowRecordRepository.GetByIdAsync(id, cancellationToken);
        return borrowRecord != null ? BorrowRecordMapper.ToDto(borrowRecord) : null;
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetAllBorrowRecordsAsync(CancellationToken cancellationToken = default)
    {
        var borrowRecords = await _borrowRecordRepository.GetAllAsync(cancellationToken);
        return borrowRecords.Select(BorrowRecordMapper.ToDto);
    }

    public async Task<bool> ReturnBookAsync(Guid borrowRecordId, CancellationToken cancellationToken = default)
    {
        var borrowRecord = await _borrowRecordRepository.GetByIdAsync(borrowRecordId, cancellationToken);
        if (borrowRecord == null)
            return false;

        // Domain service kullanarak iade işlemini yap
        await _borrowingService.ReturnBookAsync(borrowRecordId, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetBorrowRecordsByMemberAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        var borrowRecords = await _borrowRecordRepository.GetBorrowRecordsByMemberAsync(memberId, cancellationToken);
        return borrowRecords.Select(BorrowRecordMapper.ToDto);
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetBorrowRecordsByBookAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        var borrowRecords = await _borrowRecordRepository.GetBorrowRecordsByBookAsync(bookId, cancellationToken);
        return borrowRecords.Select(BorrowRecordMapper.ToDto);
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetActiveBorrowRecordsAsync(CancellationToken cancellationToken = default)
    {
        var borrowRecords = await _borrowRecordRepository.GetActiveBorrowRecordsAsync(cancellationToken);
        return borrowRecords.Select(BorrowRecordMapper.ToDto);
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetOverdueBorrowRecordsAsync(CancellationToken cancellationToken = default)
    {
        var borrowRecords = await _borrowRecordRepository.GetOverdueRecordsAsync(cancellationToken);
        return borrowRecords.Select(BorrowRecordMapper.ToDto);
    }

    public async Task<decimal> CalculateFineAsync(Guid borrowRecordId, CancellationToken cancellationToken = default)
    {
        return await _borrowingService.CalculateFineAsync(borrowRecordId, cancellationToken);
    }
}
