using LibraryApp.Application.DTOs.BorrowRecords;

namespace LibraryApp.Application.Mappers;

/// <summary>
/// Ödünç verme kaydı mapper'ı
/// BorrowRecord entity'si ile BorrowRecord DTO'ları arasında dönüşüm yapar
/// </summary>
public static class BorrowRecordMapper
{
    /// <summary>
    /// BorrowRecord entity'sini BorrowRecordDto'ya çevirir
    /// </summary>
    /// <param name="borrowRecord">BorrowRecord entity'si</param>
    /// <returns>BorrowRecordDto</returns>
    public static BorrowRecordDto ToDto(Domain.Entities.BorrowRecord borrowRecord)
    {
        return new BorrowRecordDto
        {
            Id = borrowRecord.Id,
            BorrowDate = borrowRecord.BorrowDate,
            DueDate = borrowRecord.DueDate,
            ReturnDate = borrowRecord.ReturnDate,
            Status = borrowRecord.Status,
            FineAmount = borrowRecord.FineAmount,
            Notes = borrowRecord.Notes,
            BookId = borrowRecord.BookId,
            BookTitle = borrowRecord.Book?.Title ?? string.Empty,
            MemberId = borrowRecord.MemberId,
            MemberName = borrowRecord.Member?.FullName ?? string.Empty
        };
    }

    /// <summary>
    /// CreateBorrowRecordDto'yu BorrowRecord entity'sine çevirir
    /// </summary>
    /// <param name="dto">CreateBorrowRecordDto</param>
    /// <returns>BorrowRecord entity'si</returns>
    public static Domain.Entities.BorrowRecord ToEntity(CreateBorrowRecordDto dto)
    {
        return new Domain.Entities.BorrowRecord
        {
            BookId = dto.BookId,
            MemberId = dto.MemberId,
            BorrowDate = DateTime.Now,
            DueDate = dto.DueDate,
            Status = Domain.Enums.BorrowStatus.Borrowed,
            Notes = dto.Notes
        };
    }
}
