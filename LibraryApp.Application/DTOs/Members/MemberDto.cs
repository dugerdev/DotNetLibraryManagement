namespace LibraryApp.Application.DTOs.Members;

/// <summary>
/// Üye DTO'su
/// Client'a gönderilen üye bilgileri
/// </summary>
public class MemberDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime? MembershipDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsMembershipValid { get; set; }
    public int ActiveBorrowCount { get; set; }
}
