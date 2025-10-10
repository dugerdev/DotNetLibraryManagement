using LibraryApp.Application.DTOs.Members;

namespace LibraryApp.Application.Interfaces;

/// <summary>
/// Üye application service interface'i
/// Bu interface, üye ile ilgili tüm application işlemlerini tanımlar
/// </summary>
public interface IMemberApplicationService
{
    Task<Guid> CreateMemberAsync(CreateMemberDto dto, CancellationToken cancellationToken = default);
    Task<MemberDto?> GetMemberByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MemberDto>> GetAllMembersAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateMemberAsync(Guid id, UpdateMemberDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteMemberAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MemberDto>> SearchMembersAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<MemberDto>> GetActiveMembersAsync(CancellationToken cancellationToken = default);
    Task<MemberDto?> GetMemberByEmailAsync(string email, CancellationToken cancellationToken = default);
}
