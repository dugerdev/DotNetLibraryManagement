using LibraryApp.Application.DTOs.BorrowRecords;
using LibraryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Web.Controllers;

/// <summary>
/// Ödünç verme kayıtları (BorrowRecord) yönetimi için API Controller
/// Kitap ödünç verme ve iade işlemlerini yönetir
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BorrowRecordsController : ControllerBase
{
    private readonly IBorrowRecordApplicationService _borrowRecordService;
    private readonly ILogger<BorrowRecordsController> _logger;

    public BorrowRecordsController(
        IBorrowRecordApplicationService borrowRecordService,
        ILogger<BorrowRecordsController> logger)
    {
        _borrowRecordService = borrowRecordService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm ödünç kayıtlarını getirir
    /// </summary>
    /// <returns>Ödünç kayıt listesi</returns>
    /// <response code="200">Başarılı - Kayıt listesi döner</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BorrowRecordDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetAllBorrowRecords(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all borrow records");
        var records = await _borrowRecordService.GetAllBorrowRecordsAsync(cancellationToken);
        return Ok(records);
    }

    /// <summary>
    /// ID'ye göre ödünç kaydı getirir
    /// </summary>
    /// <param name="id">Kayıt ID'si</param>
    /// <returns>Kayıt detayları</returns>
    /// <response code="200">Başarılı - Kayıt bulundu</response>
    /// <response code="404">Kayıt bulunamadı</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BorrowRecordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BorrowRecordDto>> GetBorrowRecordById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting borrow record with ID: {RecordId}", id);
        
        var record = await _borrowRecordService.GetBorrowRecordByIdAsync(id, cancellationToken);
        
        if (record == null)
        {
            _logger.LogWarning("Borrow record with ID {RecordId} not found", id);
            return NotFound(new { message = $"Borrow record with ID {id} not found" });
        }

        return Ok(record);
    }

    /// <summary>
    /// Yeni ödünç kaydı oluşturur (kitap ödünç verme)
    /// </summary>
    /// <param name="createDto">Ödünç kayıt bilgileri</param>
    /// <returns>Oluşturulan kaydın ID'si</returns>
    /// <response code="201">Başarılı - Kayıt oluşturuldu</response>
    /// <response code="400">Geçersiz veri veya iş kuralı ihlali</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateBorrowRecord(
        [FromBody] CreateBorrowRecordDto createDto, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new borrow record for Member: {MemberId}, Book: {BookId}", 
            createDto.MemberId, createDto.BookId);

        try
        {
            var recordId = await _borrowRecordService.CreateBorrowRecordAsync(createDto, cancellationToken);
            
            _logger.LogInformation("Borrow record created successfully with ID: {RecordId}", recordId);
            
            return CreatedAtAction(
                nameof(GetBorrowRecordById), 
                new { id = recordId }, 
                recordId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating borrow record");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Kitap iade işlemi yapar
    /// </summary>
    /// <param name="id">Ödünç kayıt ID'si</param>
    /// <returns>İade sonucu</returns>
    /// <response code="204">Başarılı - Kitap iade edildi</response>
    /// <response code="404">Kayıt bulunamadı</response>
    /// <response code="400">İş kuralı ihlali</response>
    [HttpPut("{id:guid}/return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReturnBook(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing book return for borrow record ID: {RecordId}", id);

        try
        {
            var result = await _borrowRecordService.ReturnBookAsync(id, cancellationToken);
            
            if (!result)
            {
                _logger.LogWarning("Borrow record with ID {RecordId} not found for return", id);
                return NotFound(new { message = $"Borrow record with ID {id} not found" });
            }

            _logger.LogInformation("Book returned successfully for record ID: {RecordId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error returning book for record ID: {RecordId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Üyeye ait tüm ödünç kayıtlarını getirir
    /// </summary>
    /// <param name="memberId">Üye ID'si</param>
    /// <returns>Üyenin ödünç kayıtları</returns>
    /// <response code="200">Başarılı - Kayıt listesi</response>
    [HttpGet("member/{memberId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<BorrowRecordDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetBorrowRecordsByMember(
        Guid memberId, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting borrow records for member: {MemberId}", memberId);
        
        var records = await _borrowRecordService.GetBorrowRecordsByMemberAsync(memberId, cancellationToken);
        return Ok(records);
    }

    /// <summary>
    /// Kitaba ait tüm ödünç kayıtlarını getirir (kitabın geçmişi)
    /// </summary>
    /// <param name="bookId">Kitap ID'si</param>
    /// <returns>Kitabın ödünç kayıtları</returns>
    /// <response code="200">Başarılı - Kayıt listesi</response>
    [HttpGet("book/{bookId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<BorrowRecordDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetBorrowRecordsByBook(
        Guid bookId, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting borrow records for book: {BookId}", bookId);
        
        var records = await _borrowRecordService.GetBorrowRecordsByBookAsync(bookId, cancellationToken);
        return Ok(records);
    }

    /// <summary>
    /// Aktif ödünç kayıtlarını getirir (henüz iade edilmemiş kitaplar)
    /// </summary>
    /// <returns>Aktif kayıt listesi</returns>
    /// <response code="200">Başarılı - Aktif kayıtlar</response>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<BorrowRecordDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetActiveBorrowRecords(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting active borrow records");
        
        var records = await _borrowRecordService.GetActiveBorrowRecordsAsync(cancellationToken);
        return Ok(records);
    }

    /// <summary>
    /// Gecikmiş ödünç kayıtlarını getirir (iade tarihi geçmiş)
    /// </summary>
    /// <returns>Gecikmiş kayıt listesi</returns>
    /// <response code="200">Başarılı - Gecikmiş kayıtlar</response>
    [HttpGet("overdue")]
    [ProducesResponseType(typeof(IEnumerable<BorrowRecordDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetOverdueBorrowRecords(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting overdue borrow records");
        
        var records = await _borrowRecordService.GetOverdueBorrowRecordsAsync(cancellationToken);
        return Ok(records);
    }

    /// <summary>
    /// Ödünç kaydı için ceza miktarını hesaplar
    /// </summary>
    /// <param name="id">Kayıt ID'si</param>
    /// <returns>Ceza miktarı (TL)</returns>
    /// <response code="200">Başarılı - Ceza miktarı</response>
    /// <response code="404">Kayıt bulunamadı</response>
    [HttpGet("{id:guid}/fine")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<decimal>> CalculateFine(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Calculating fine for borrow record ID: {RecordId}", id);
        
        try
        {
            var fine = await _borrowRecordService.CalculateFineAsync(id, cancellationToken);
            return Ok(fine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating fine for record ID: {RecordId}", id);
            return NotFound(new { message = ex.Message });
        }
    }
}

