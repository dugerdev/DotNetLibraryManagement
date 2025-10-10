using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Domain.Common;

/// <summary>
/// Category entity'sine özel repository interface'i
/// Kategorilerle ilgili domain-specific işlemleri tanımlar
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    // Category-specific queries - Kategoriye özel sorgular
    Task<Category?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetCategoriesWithBooksAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetEmptyCategoriesAsync(CancellationToken cancellationToken = default);

    // Statistics - İstatistikler
    Task<int> GetCategoryCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetCategoriesWithBooksCountAsync(CancellationToken cancellationToken = default);

    // Validation - Doğrulama
    Task<bool> IsCategoryNameUniqueAsync(string name, Guid? excludeCategoryId = null, CancellationToken cancellationToken = default);
}