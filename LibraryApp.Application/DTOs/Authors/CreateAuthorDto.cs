namespace LibraryApp.Application.DTOs.Authors;

/// <summary>
/// Yazar oluşturma DTO'su
/// Client'tan gelen yeni yazar verisi
/// </summary>
public class CreateAuthorDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }
}
