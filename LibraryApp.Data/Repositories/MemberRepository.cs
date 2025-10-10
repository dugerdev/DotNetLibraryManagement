using LibraryApp.Data.Context;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data.Repositories;

public class MemberRepository 
{
    private readonly LibraryDbContext _context;

    public MemberRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<Member> AddAsync(Member member)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task<Member?> GetByIdAsync(Guid id)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
    }

    public async Task<Member?> GetByEmailAsync(string email)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Email == email && !m.IsDeleted);
    }

    public async Task<List<Member>> GetAllAsync()
    {
        return await _context.Members
            .Where(m => !m.IsDeleted)
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToListAsync();
    }

    public async Task<List<Member>> GetActiveMembersAsync()
    {
        return await _context.Members
            .Where(m => !m.IsDeleted && m.IsActive)
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToListAsync();
    }

    public async Task<Member> UpdateAsync(Member member)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task<Member> DeleteAsync(Member member)
    {
        member.IsDeleted = true;
        member.DeletedOn = DateTimeOffset.Now;

        _context.Members.Update(member);
        await _context.SaveChangesAsync();

        return member;
    }

    public async Task<List<BorrowRecord>> GetBorrowRecordsAsync(Guid memberId)
    {
        return await _context.BorrowRecords
            .Include(br => br.Book)                    
            .ThenInclude(b => b.Author)                
            .Include(br => br.Book)                    
            .ThenInclude(b => b.Category)              
            .Where(br => br.MemberId == memberId && !br.IsDeleted)
            .OrderByDescending(br => br.BorrowDate)   
            .ToListAsync();
    }
}
