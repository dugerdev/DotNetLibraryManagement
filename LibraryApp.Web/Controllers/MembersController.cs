using LibraryApp.Application.DTOs.Members;
using LibraryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Web.Controllers;

/// <summary>
/// Üye (Member) yönetimi için API Controller
/// Kütüphane üyelerinin CRUD operasyonlarını yönetir
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MembersController : ControllerBase
{
    private readonly IMemberApplicationService _memberService;
    private readonly ILogger<MembersController> _logger;

    public MembersController(
        IMemberApplicationService memberService,
        ILogger<MembersController> logger)
    {
        _memberService = memberService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm üyeleri getirir
    /// </summary>
    /// <returns>Üye listesi</returns>
    /// <response code="200">Başarılı - Üye listesi döner</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MemberDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all members");
        var members = await _memberService.GetAllMembersAsync(cancellationToken);
        return Ok(members);
    }

    /// <summary>
    /// ID'ye göre üye getirir
    /// </summary>
    /// <param name="id">Üye ID'si</param>
    /// <returns>Üye detayları</returns>
    /// <response code="200">Başarılı - Üye bulundu</response>
    /// <response code="404">Üye bulunamadı</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MemberDto>> GetMemberById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting member with ID: {MemberId}", id);
        
        var member = await _memberService.GetMemberByIdAsync(id, cancellationToken);
        
        if (member == null)
        {
            _logger.LogWarning("Member with ID {MemberId} not found", id);
            return NotFound(new { message = $"Member with ID {id} not found" });
        }

        return Ok(member);
    }

    /// <summary>
    /// Yeni üye oluşturur
    /// </summary>
    /// <param name="createDto">Üye oluşturma bilgileri</param>
    /// <returns>Oluşturulan üyenin ID'si</returns>
    /// <response code="201">Başarılı - Üye oluşturuldu</response>
    /// <response code="400">Geçersiz veri</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateMember(
        [FromBody] CreateMemberDto createDto, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new member: {FirstName} {LastName}", 
            createDto.FirstName, createDto.LastName);

        try
        {
            var memberId = await _memberService.CreateMemberAsync(createDto, cancellationToken);
            
            _logger.LogInformation("Member created successfully with ID: {MemberId}", memberId);
            
            return CreatedAtAction(
                nameof(GetMemberById), 
                new { id = memberId }, 
                memberId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating member");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Üye bilgilerini günceller
    /// </summary>
    /// <param name="id">Üye ID'si</param>
    /// <param name="updateDto">Güncellenmiş üye bilgileri</param>
    /// <returns>Güncelleme sonucu</returns>
    /// <response code="204">Başarılı - Üye güncellendi</response>
    /// <response code="404">Üye bulunamadı</response>
    /// <response code="400">Geçersiz veri</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMember(
        Guid id, 
        [FromBody] UpdateMemberDto updateDto, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating member with ID: {MemberId}", id);

        try
        {
            var result = await _memberService.UpdateMemberAsync(id, updateDto, cancellationToken);
            
            if (!result)
            {
                _logger.LogWarning("Member with ID {MemberId} not found for update", id);
                return NotFound(new { message = $"Member with ID {id} not found" });
            }

            _logger.LogInformation("Member with ID {MemberId} updated successfully", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating member with ID: {MemberId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Üye siler (soft delete)
    /// </summary>
    /// <param name="id">Üye ID'si</param>
    /// <returns>Silme sonucu</returns>
    /// <response code="204">Başarılı - Üye silindi</response>
    /// <response code="404">Üye bulunamadı</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMember(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting member with ID: {MemberId}", id);

        var result = await _memberService.DeleteMemberAsync(id, cancellationToken);
        
        if (!result)
        {
            _logger.LogWarning("Member with ID {MemberId} not found for deletion", id);
            return NotFound(new { message = $"Member with ID {id} not found" });
        }

        _logger.LogInformation("Member with ID {MemberId} deleted successfully", id);
        return NoContent();
    }

    /// <summary>
    /// Üye arama (isim, soyisim, email veya telefon)
    /// </summary>
    /// <param name="searchTerm">Arama terimi</param>
    /// <returns>Bulunan üyeler</returns>
    /// <response code="200">Başarılı - Arama sonuçları</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<MemberDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MemberDto>>> SearchMembers(
        [FromQuery] string searchTerm, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching members with term: {SearchTerm}", searchTerm);
        
        var members = await _memberService.SearchMembersAsync(searchTerm, cancellationToken);
        return Ok(members);
    }

    /// <summary>
    /// Aktif üyeleri getirir (üyeliği geçerli olanlar)
    /// </summary>
    /// <returns>Aktif üye listesi</returns>
    /// <response code="200">Başarılı - Aktif üyeler</response>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<MemberDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetActiveMembers(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting active members");
        
        var members = await _memberService.GetActiveMembersAsync(cancellationToken);
        return Ok(members);
    }

    /// <summary>
    /// Email adresine göre üye getirir
    /// </summary>
    /// <param name="email">Email adresi</param>
    /// <returns>Üye bilgileri</returns>
    /// <response code="200">Başarılı - Üye bulundu</response>
    /// <response code="404">Üye bulunamadı</response>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(MemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MemberDto>> GetMemberByEmail(string email, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting member with email: {Email}", email);
        
        var member = await _memberService.GetMemberByEmailAsync(email, cancellationToken);
        
        if (member == null)
        {
            _logger.LogWarning("Member with email {Email} not found", email);
            return NotFound(new { message = $"Member with email {email} not found" });
        }

        return Ok(member);
    }
}

