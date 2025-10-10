namespace LibraryApp.Application.DTOs.Members;

/// <summary>
/// Üye oluşturma DTO'su
/// Client'tan gelen yeni üye verisi
/// </summary>
public class CreateMemberDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime? MembershipDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
