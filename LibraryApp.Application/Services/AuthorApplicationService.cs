using LibraryApp.Application.DTOs.Authors;
using LibraryApp.Application.Interfaces;
using LibraryApp.Application.Mappers;
using LibraryApp.Domain.Common;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Application.Services;

/// <summary>
/// Yazar application service implementasyonu
/// </summary>
public class AuthorApplicationService : IAuthorApplicationService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthorApplicationService(
        IAuthorRepository authorRepository,
        IUnitOfWork unitOfWork)
    {
        _authorRepository = authorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateAuthorAsync(CreateAuthorDto dto, CancellationToken cancellationToken = default)
    {
        // Ad-soyad benzersizlik kontrolü
        var existingAuthor = await _authorRepository.GetAuthorByNameAsync(dto.FirstName, dto.LastName, cancellationToken);
        if (existingAuthor != null)
            throw new DuplicateEntityException("Author", "Name", $"{dto.FirstName} {dto.LastName}");

        // Entity oluştur
        var author = AuthorMapper.ToEntity(dto);
        var createdAuthor = await _authorRepository.AddAsync(author, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return createdAuthor.Id;
    }

    public async Task<AuthorDto?> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken);
        return author != null ? AuthorMapper.ToDto(author) : null;
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(CancellationToken cancellationToken = default)
    {
        var authors = await _authorRepository.GetAllAsync(cancellationToken);
        return authors.Select(AuthorMapper.ToDto);
    }

    public async Task<bool> UpdateAuthorAsync(Guid id, UpdateAuthorDto dto, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken);
        if (author == null)
            return false;

        // Ad-soyad benzersizlik kontrolü (kendisi hariç)
        var existingAuthor = await _authorRepository.GetAuthorByNameAsync(dto.FirstName, dto.LastName, cancellationToken);
        if (existingAuthor != null && existingAuthor.Id != id)
            throw new DuplicateEntityException("Author", "Name", $"{dto.FirstName} {dto.LastName}");

        // Güncelle
        AuthorMapper.UpdateEntity(author, dto);
        await _authorRepository.UpdateAsync(author, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAuthorAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(id, cancellationToken);
        if (author == null)
            return false;

        await _authorRepository.SoftDeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<AuthorDto>> SearchAuthorsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var authors = await _authorRepository.SearchAuthorsAsync(searchTerm, cancellationToken);
        return authors.Select(AuthorMapper.ToDto);
    }

    public async Task<IEnumerable<AuthorDto>> GetAuthorsWithBooksAsync(CancellationToken cancellationToken = default)
    {
        var authors = await _authorRepository.GetAuthorsWithBooksAsync(cancellationToken);
        return authors.Select(AuthorMapper.ToDto);
    }
}
