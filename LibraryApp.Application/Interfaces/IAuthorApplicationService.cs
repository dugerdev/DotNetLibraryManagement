using LibraryApp.Application.DTOs.Authors;

namespace LibraryApp.Application.Interfaces;

/// <summary>
/// Yazar application service interface'i
/// Bu interface, yazar ile ilgili tüm application işlemlerini tanımlar
/// </summary>
public interface IAuthorApplicationService
{
    Task<Guid> CreateAuthorAsync(CreateAuthorDto dto, CancellationToken cancellationToken = default);
    Task<AuthorDto?> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAuthorAsync(Guid id, UpdateAuthorDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAuthorAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuthorDto>> SearchAuthorsAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuthorDto>> GetAuthorsWithBooksAsync(CancellationToken cancellationToken = default);
}
