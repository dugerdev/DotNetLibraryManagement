using LibraryApp.Application.DTOs.Categories;

namespace LibraryApp.Application.Mappers;

/// <summary>
/// Kategori mapper'ı
/// Category entity'si ile Category DTO'ları arasında dönüşüm yapar
/// </summary>
public static class CategoryMapper
{
    /// <summary>
    /// Category entity'sini CategoryDto'ya çevirir
    /// </summary>
    /// <param name="category">Category entity'si</param>
    /// <returns>CategoryDto</returns>
    public static CategoryDto ToDto(Domain.Entities.Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            BookCount = category.BookCount
        };
    }

    /// <summary>
    /// CreateCategoryDto'yu Category entity'sine çevirir
    /// </summary>
    /// <param name="dto">CreateCategoryDto</param>
    /// <returns>Category entity'si</returns>
    public static Domain.Entities.Category ToEntity(CreateCategoryDto dto)
    {
        return new Domain.Entities.Category
        {
            Name = dto.Name,
            Description = dto.Description
        };
    }

    /// <summary>
    /// UpdateCategoryDto'yu mevcut Category entity'sine uygular
    /// </summary>
    /// <param name="category">Mevcut Category entity'si</param>
    /// <param name="dto">UpdateCategoryDto</param>
    public static void UpdateEntity(Domain.Entities.Category category, UpdateCategoryDto dto)
    {
        category.Name = dto.Name;
        category.Description = dto.Description;
    }
}
