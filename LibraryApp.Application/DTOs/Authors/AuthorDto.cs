namespace LibraryApp.Application.DTOs.Authors;

/// <summary>
/// Yazar DTO'su
/// Client'a gönderilen yazar bilgileri
/// Domain entity'den farklı olarak sadece client'ın ihtiyacı olan alanları içerir
/// </summary>
public class AuthorDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? Age { get; set; }
    public int BookCount { get; set; }
}
