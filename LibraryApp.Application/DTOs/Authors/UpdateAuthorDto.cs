namespace LibraryApp.Application.DTOs.Authors;

/// <summary>
/// Yazar güncelleme DTO'su
/// Client'tan gelen yazar güncelleme verisi
/// </summary>
public class UpdateAuthorDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }
}
