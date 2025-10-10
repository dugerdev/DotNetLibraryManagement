namespace LibraryApp.Application.DTOs.Categories;

/// <summary>
/// Kategori güncelleme DTO'su
/// Client'tan gelen kategori güncelleme verisi
/// </summary>
public class UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
