using LibraryApp.Application.DTOs.Categories;

namespace LibraryApp.Application.Interfaces;

/// <summary>
/// Kategori application service interface'i
/// Bu interface, kategori ile ilgili tüm application işlemlerini tanımlar
/// </summary>
public interface ICategoryApplicationService
{
    Task<Guid> CreateCategoryAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> SearchCategoriesAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryDto>> GetCategoriesWithBooksAsync(CancellationToken cancellationToken = default);
}
