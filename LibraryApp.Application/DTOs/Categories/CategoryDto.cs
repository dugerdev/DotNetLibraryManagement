namespace LibraryApp.Application.DTOs.Categories;

/// <summary>
/// Kategori DTO'su
/// Client'a gönderilen kategori bilgileri
/// </summary>
public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int BookCount { get; set; }
}
