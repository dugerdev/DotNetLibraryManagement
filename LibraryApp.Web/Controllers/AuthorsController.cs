using LibraryApp.Application.DTOs.Authors;
using LibraryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Web.Controllers;

/// <summary>
/// Yazar (Author) yönetimi için API Controller
/// Bu controller, yazar CRUD operasyonlarını yönetir
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorApplicationService _authorService;
    private readonly ILogger<AuthorsController> _logger;

    public AuthorsController(
        IAuthorApplicationService authorService,
        ILogger<AuthorsController> logger)
    {
        _authorService = authorService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm yazarları getirir
    /// </summary>
    /// <returns>Yazar listesi</returns>
    /// <response code="200">Başarılı - Yazar listesi döner</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AuthorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAllAuthors(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all authors");
        var authors = await _authorService.GetAllAuthorsAsync(cancellationToken);
        return Ok(authors);
    }

    /// <summary>
    /// ID'ye göre yazar getirir
    /// </summary>
    /// <param name="id">Yazar ID'si</param>
    /// <returns>Yazar detayları</returns>
    /// <response code="200">Başarılı - Yazar bulundu</response>
    /// <response code="404">Yazar bulunamadı</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthorDto>> GetAuthorById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting author with ID: {AuthorId}", id);
        
        var author = await _authorService.GetAuthorByIdAsync(id, cancellationToken);
        
        if (author == null)
        {
            _logger.LogWarning("Author with ID {AuthorId} not found", id);
            return NotFound(new { message = $"Author with ID {id} not found" });
        }

        return Ok(author);
    }

    /// <summary>
    /// Yeni yazar oluşturur
    /// </summary>
    /// <param name="createDto">Yazar oluşturma bilgileri</param>
    /// <returns>Oluşturulan yazarın ID'si</returns>
    /// <response code="201">Başarılı - Yazar oluşturuldu</response>
    /// <response code="400">Geçersiz veri</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateAuthor(
        [FromBody] CreateAuthorDto createDto, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new author: {FirstName} {LastName}", 
            createDto.FirstName, createDto.LastName);

        try
        {
            var authorId = await _authorService.CreateAuthorAsync(createDto, cancellationToken);
            
            _logger.LogInformation("Author created successfully with ID: {AuthorId}", authorId);
            
            return CreatedAtAction(
                nameof(GetAuthorById), 
                new { id = authorId }, 
                authorId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating author");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Yazar bilgilerini günceller
    /// </summary>
    /// <param name="id">Yazar ID'si</param>
    /// <param name="updateDto">Güncellenmiş yazar bilgileri</param>
    /// <returns>Güncelleme sonucu</returns>
    /// <response code="204">Başarılı - Yazar güncellendi</response>
    /// <response code="404">Yazar bulunamadı</response>
    /// <response code="400">Geçersiz veri</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuthor(
        Guid id, 
        [FromBody] UpdateAuthorDto updateDto, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating author with ID: {AuthorId}", id);

        try
        {
            var result = await _authorService.UpdateAuthorAsync(id, updateDto, cancellationToken);
            
            if (!result)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found for update", id);
                return NotFound(new { message = $"Author with ID {id} not found" });
            }

            _logger.LogInformation("Author with ID {AuthorId} updated successfully", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating author with ID: {AuthorId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Yazar siler (soft delete)
    /// </summary>
    /// <param name="id">Yazar ID'si</param>
    /// <returns>Silme sonucu</returns>
    /// <response code="204">Başarılı - Yazar silindi</response>
    /// <response code="404">Yazar bulunamadı</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuthor(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting author with ID: {AuthorId}", id);

        var result = await _authorService.DeleteAuthorAsync(id, cancellationToken);
        
        if (!result)
        {
            _logger.LogWarning("Author with ID {AuthorId} not found for deletion", id);
            return NotFound(new { message = $"Author with ID {id} not found" });
        }

        _logger.LogInformation("Author with ID {AuthorId} deleted successfully", id);
        return NoContent();
    }

    /// <summary>
    /// Yazar arama (isim, soyisim veya biyografi)
    /// </summary>
    /// <param name="searchTerm">Arama terimi</param>
    /// <returns>Bulunan yazarlar</returns>
    /// <response code="200">Başarılı - Arama sonuçları</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<AuthorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> SearchAuthors(
        [FromQuery] string searchTerm, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching authors with term: {SearchTerm}", searchTerm);
        
        var authors = await _authorService.SearchAuthorsAsync(searchTerm, cancellationToken);
        return Ok(authors);
    }

    /// <summary>
    /// Kitabı olan yazarları getirir
    /// </summary>
    /// <returns>Kitabı olan yazar listesi</returns>
    /// <response code="200">Başarılı - Kitabı olan yazarlar</response>
    [HttpGet("with-books")]
    [ProducesResponseType(typeof(IEnumerable<AuthorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthorsWithBooks(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting authors with books");
        
        var authors = await _authorService.GetAuthorsWithBooksAsync(cancellationToken);
        return Ok(authors);
    }
}

