using LibraryApp.Domain.Entities;

namespace LibraryApp.Domain.Common;

/// <summary>
/// Author entity'sine özel repository interface'i
/// Yazarlarla ilgili domain-specific işlemleri tanımlar
/// </summary>
public interface IAuthorRepository : IRepository<Author>
{
    // Author-specific queries - Yazara özel sorgular
    Task<IEnumerable<Author>> GetAuthorsByNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default);
    Task<IEnumerable<Author>> SearchAuthorsAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Author>> GetAuthorsWithBooksAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Author>> GetAuthorsByBirthYearAsync(int birthYear, CancellationToken cancellationToken = default);
    Task<Author?> GetAuthorByNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default);
    
    // Statistics - İstatistikler
    Task<int> GetAuthorCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetAuthorsWithBooksCountAsync(CancellationToken cancellationToken = default);
    
    // Validation - Doğrulama
    Task<bool> IsAuthorNameUniqueAsync(string firstName, string lastName, Guid? excludeAuthorId = null, CancellationToken cancellationToken = default);
}
