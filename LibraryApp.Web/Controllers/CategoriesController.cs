using LibraryApp.Application.DTOs.Categories;
using LibraryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Web.Controllers;

/// <summary>
/// Kategori (Category) yönetimi için API Controller
/// Kitap kategorilerinin CRUD operasyonlarını yönetir
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryApplicationService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(
        ICategoryApplicationService categoryService,
        ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm kategorileri getirir
    /// </summary>
    /// <returns>Kategori listesi</returns>
    /// <response code="200">Başarılı - Kategori listesi döner</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all categories");
        var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);
        return Ok(categories);
    }

    /// <summary>
    /// ID'ye göre kategori getirir
    /// </summary>
    /// <param name="id">Kategori ID'si</param>
    /// <returns>Kategori detayları</returns>
    /// <response code="200">Başarılı - Kategori bulundu</response>
    /// <response code="404">Kategori bulunamadı</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting category with ID: {CategoryId}", id);
        
        var category = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
        
        if (category == null)
        {
            _logger.LogWarning("Category with ID {CategoryId} not found", id);
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        return Ok(category);
    }

    /// <summary>
    /// Yeni kategori oluşturur
    /// </summary>
    /// <param name="createDto">Kategori oluşturma bilgileri</param>
    /// <returns>Oluşturulan kategorinin ID'si</returns>
    /// <response code="201">Başarılı - Kategori oluşturuldu</response>
    /// <response code="400">Geçersiz veri</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateCategory(
        [FromBody] CreateCategoryDto createDto, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new category: {Name}", createDto.Name);

        try
        {
            var categoryId = await _categoryService.CreateCategoryAsync(createDto, cancellationToken);
            
            _logger.LogInformation("Category created successfully with ID: {CategoryId}", categoryId);
            
            return CreatedAtAction(
                nameof(GetCategoryById), 
                new { id = categoryId }, 
                categoryId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Kategori bilgilerini günceller
    /// </summary>
    /// <param name="id">Kategori ID'si</param>
    /// <param name="updateDto">Güncellenmiş kategori bilgileri</param>
    /// <returns>Güncelleme sonucu</returns>
    /// <response code="204">Başarılı - Kategori güncellendi</response>
    /// <response code="404">Kategori bulunamadı</response>
    /// <response code="400">Geçersiz veri</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCategory(
        Guid id, 
        [FromBody] UpdateCategoryDto updateDto, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating category with ID: {CategoryId}", id);

        try
        {
            var result = await _categoryService.UpdateCategoryAsync(id, updateDto, cancellationToken);
            
            if (!result)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found for update", id);
                return NotFound(new { message = $"Category with ID {id} not found" });
            }

            _logger.LogInformation("Category with ID {CategoryId} updated successfully", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category with ID: {CategoryId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Kategori siler (soft delete)
    /// </summary>
    /// <param name="id">Kategori ID'si</param>
    /// <returns>Silme sonucu</returns>
    /// <response code="204">Başarılı - Kategori silindi</response>
    /// <response code="404">Kategori bulunamadı</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting category with ID: {CategoryId}", id);

        var result = await _categoryService.DeleteCategoryAsync(id, cancellationToken);
        
        if (!result)
        {
            _logger.LogWarning("Category with ID {CategoryId} not found for deletion", id);
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        _logger.LogInformation("Category with ID {CategoryId} deleted successfully", id);
        return NoContent();
    }

    /// <summary>
    /// Kategori arama (isim veya açıklama)
    /// </summary>
    /// <param name="searchTerm">Arama terimi</param>
    /// <returns>Bulunan kategoriler</returns>
    /// <response code="200">Başarılı - Arama sonuçları</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> SearchCategories(
        [FromQuery] string searchTerm, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching categories with term: {SearchTerm}", searchTerm);
        
        var categories = await _categoryService.SearchCategoriesAsync(searchTerm, cancellationToken);
        return Ok(categories);
    }

    /// <summary>
    /// Kitabı olan kategorileri getirir
    /// </summary>
    /// <returns>Kitabı olan kategori listesi</returns>
    /// <response code="200">Başarılı - Kitabı olan kategoriler</response>
    [HttpGet("with-books")]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesWithBooks(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting categories with books");
        
        var categories = await _categoryService.GetCategoriesWithBooksAsync(cancellationToken);
        return Ok(categories);
    }
}
