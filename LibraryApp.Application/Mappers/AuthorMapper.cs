using LibraryApp.Application.DTOs.Authors;

namespace LibraryApp.Application.Mappers;

/// <summary>
/// Yazar mapper'ı
/// Author entity'si ile Author DTO'ları arasında dönüşüm yapar
/// </summary>
public static class AuthorMapper
{
    /// <summary>
    /// Author entity'sini AuthorDto'ya çevirir
    /// </summary>
    /// <param name="author">Author entity'si</param>
    /// <returns>AuthorDto</returns>
    public static AuthorDto ToDto(Domain.Entities.Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            FullName = author.FullName,
            Biography = author.Biography,
            BirthDate = author.BirthDate,
            Age = author.Age,
            BookCount = author.Books.Count
        };
    }

    /// <summary>
    /// CreateAuthorDto'yu Author entity'sine çevirir
    /// </summary>
    /// <param name="dto">CreateAuthorDto</param>
    /// <returns>Author entity'si</returns>
    public static Domain.Entities.Author ToEntity(CreateAuthorDto dto)
    {
        return new Domain.Entities.Author
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Biography = dto.Biography,
            BirthDate = dto.BirthDate
        };
    }

    /// <summary>
    /// UpdateAuthorDto'yu mevcut Author entity'sine uygular
    /// </summary>
    /// <param name="author">Mevcut Author entity'si</param>
    /// <param name="dto">UpdateAuthorDto</param>
    public static void UpdateEntity(Domain.Entities.Author author, UpdateAuthorDto dto)
    {
        author.FirstName = dto.FirstName;
        author.LastName = dto.LastName;
        author.Biography = dto.Biography;
        author.BirthDate = dto.BirthDate;
    }
}
