namespace LibraryApp.Application.DTOs.Categories;

/// <summary>
/// Kategori oluşturma DTO'su
/// Client'tan gelen yeni kategori verisi
/// </summary>
public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
