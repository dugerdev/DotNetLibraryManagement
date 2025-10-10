using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Domain.Common;

/// <summary>
/// Member entity'sine özel repository interface'i
/// Üyelerle ilgili domain-specific işlemleri tanımlar
/// </summary>
public interface IMemberRepository : IRepository<Member>
{
    // Member-specific queries - Üyeye özel sorgular
    Task<Member?> GetMemberByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Member?> GetMemberByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Member>> GetActiveMembersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Member>> GetExpiredMembersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Member>> GetMembersWithOverdueBooksAsync(CancellationToken cancellationToken = default);

    // Membership operations - Üyelik işlemleri
    Task<bool> IsEmailUniqueAsync(string email, Guid? excludeMemberId = null, CancellationToken cancellationToken = default);
    Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, Guid? excludeMemberId = null, CancellationToken cancellationToken = default);
    Task<bool> IsMembershipValidAsync(Guid memberId, CancellationToken cancellationToken = default);

    // Statistics - İstatistikler
    Task<int> GetActiveMemberCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetExpiredMemberCountAsync(CancellationToken cancellationToken = default);
}