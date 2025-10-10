namespace LibraryApp.Application.DTOs.Members;

/// <summary>
/// Üye güncelleme DTO'su
/// Client'tan gelen üye güncelleme verisi
/// </summary>
public class UpdateMemberDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime? MembershipDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsActive { get; set; }
}
