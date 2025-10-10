namespace LibraryApp.Application.DTOs.BorrowRecords;

/// <summary>
/// Ödünç verme kaydı oluşturma DTO'su
/// Client'tan gelen yeni ödünç verme verisi
/// </summary>
public class CreateBorrowRecordDto
{
    public Guid BookId { get; set; }
    public Guid MemberId { get; set; }
    public DateTime DueDate { get; set; }
    public string? Notes { get; set; }
}
