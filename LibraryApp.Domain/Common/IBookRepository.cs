using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Domain.Common;

/// <summary>
/// Book entity'sine özel repository interface'i
/// Kitaplarla ilgili domain-specific işlemleri tanımlar
/// </summary>
public interface IBookRepository : IRepository<Book>
{
    // Book-specific queries - Kitaba özel sorgular
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(Guid authorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetBooksByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetAvailableBooksAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<Book?> GetBookByIsbnAsync(string isbn, CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetOverdueBooksAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetPopularBooksAsync(int count = 10, CancellationToken cancellationToken = default);
    Task<IEnumerable<Book>> GetBooksByPublishedYearAsync(int year, CancellationToken cancellationToken = default);

    // Availability operations - Müsaitlik işlemleri
    Task<bool> IsBookAvailableAsync(Guid bookId, CancellationToken cancellationToken = default);
    Task<int> GetAvailableCopiesCountAsync(Guid bookId, CancellationToken cancellationToken = default);
    Task UpdateAvailableCopiesAsync(Guid bookId, int newCount, CancellationToken cancellationToken = default);

    // Validation - Doğrulama
    Task<bool> IsIsbnUniqueAsync(string isbn, Guid? excludeBookId = null, CancellationToken cancellationToken = default);

    // Statistics - İstatistikler
    Task<int> GetTotalBookCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetAvailableBookCountAsync(CancellationToken cancellationToken = default);
}