using LibraryApp.Domain.Enums;

namespace LibraryApp.Application.DTOs.BorrowRecords;

/// <summary>
/// Ödünç verme kaydı DTO'su
/// Client'a gönderilen ödünç verme kaydı bilgileri
/// </summary>
public class BorrowRecordDto
{
    public Guid Id { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public BorrowStatus Status { get; set; }
    public decimal? FineAmount { get; set; }
    public string? Notes { get; set; }
    
    // Navigation properties
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
}
