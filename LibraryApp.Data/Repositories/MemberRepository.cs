using LibraryApp.Data.Context;
using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApp.Data.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly LibraryDbContext _context;

    public MemberRepository(LibraryDbContext context)
    {
        _context = context;
    }

    // IRepository<Member> - Base methods
    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Member>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .Where(m => !m.IsDeleted)
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Member>> FindAsync(Expression<Func<Member, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .Where(predicate)
            .Where(m => !m.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<Member?> FirstOrDefaultAsync(Expression<Func<Member, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .Where(m => !m.IsDeleted)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<Member, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .Where(m => !m.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Member, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .Where(m => !m.IsDeleted)
            .CountAsync(predicate, cancellationToken);
    }

    public async Task<(IEnumerable<Member> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<Member, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Members.Where(m => !m.IsDeleted);

        if (predicate != null)
            query = query.Where(predicate);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Member> AddAsync(Member entity, CancellationToken cancellationToken = default)
    {
        await _context.Members.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<Member>> AddRangeAsync(IEnumerable<Member> entities, CancellationToken cancellationToken = default)
    {
        await _context.Members.AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public Task UpdateAsync(Member entity, CancellationToken cancellationToken = default)
    {
        _context.Members.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Member entity, CancellationToken cancellationToken = default)
    {
        _context.Members.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.Members.Remove(entity);
        }
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTimeOffset.Now;
            _context.Members.Update(entity);
        }
    }

    public Task UpdateRangeAsync(IEnumerable<Member> entities, CancellationToken cancellationToken = default)
    {
        _context.Members.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<Member> entities, CancellationToken cancellationToken = default)
    {
        _context.Members.RemoveRange(entities);
        return Task.CompletedTask;
    }

    // IMemberRepository - Member-specific methods
    public async Task<Member?> GetMemberByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => !m.IsDeleted && m.Email == email, cancellationToken);
    }

    public async Task<Member?> GetMemberByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => !m.IsDeleted && m.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<IEnumerable<Member>> GetActiveMembersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .Where(m => !m.IsDeleted && m.IsActive && 
                       (m.ExpirationDate == null || m.ExpirationDate > DateTime.Now))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Member>> GetExpiredMembersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .Where(m => !m.IsDeleted && 
                       (m.ExpirationDate.HasValue && m.ExpirationDate <= DateTime.Now))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Member>> SearchMembersAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .Where(m => !m.IsDeleted &&
                       (m.FirstName.Contains(searchTerm) ||
                        m.LastName.Contains(searchTerm) ||
                        m.Email.Contains(searchTerm) ||
                        (m.PhoneNumber != null && m.PhoneNumber.Contains(searchTerm))))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Member>> GetMembersWithOverdueBooksAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .Include(m => m.BorrowRecords)
            .Where(m => !m.IsDeleted &&
                       m.BorrowRecords.Any(br => !br.IsDeleted &&
                                                 br.Status == Domain.Enums.BorrowStatus.Borrowed &&
                                                 br.DueDate < DateTimeOffset.Now))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeMemberId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Members
            .Where(m => !m.IsDeleted && m.Email == email);

        if (excludeMemberId.HasValue)
            query = query.Where(m => m.Id != excludeMemberId.Value);

        return !await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, Guid? excludeMemberId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Members
            .Where(m => !m.IsDeleted && m.PhoneNumber == phoneNumber);

        if (excludeMemberId.HasValue)
            query = query.Where(m => m.Id != excludeMemberId.Value);

        return !await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> IsMembershipValidAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        var member = await GetByIdAsync(memberId, cancellationToken);
        return member != null && member.IsActive && 
               (member.ExpirationDate == null || member.ExpirationDate > DateTime.Now);
    }

    public async Task<int> GetActiveMemberCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .CountAsync(m => !m.IsDeleted && m.IsActive && 
                           (m.ExpirationDate == null || m.ExpirationDate > DateTime.Now), cancellationToken);
    }

    public async Task<int> GetExpiredMemberCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .CountAsync(m => !m.IsDeleted && 
                           (m.ExpirationDate.HasValue && m.ExpirationDate <= DateTime.Now), cancellationToken);
    }
}
