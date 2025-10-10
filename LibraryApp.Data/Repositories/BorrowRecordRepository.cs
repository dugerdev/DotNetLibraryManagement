using LibraryApp.Data.Context;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data.Repositories;

public class BorrowRecordRepository
{
    private readonly LibraryDbContext _context;

    public BorrowRecordRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<BorrowRecord> AddAsync(BorrowRecord borrowRecord)
    {
        _context.BorrowRecords.Add(borrowRecord);
        _context.SaveChangesAsync();
        return borrowRecord;
    }

    public async Task<BorrowRecord?> GetByIdAsync(Guid id)
    {
        return await _context.BorrowRecords
            .Include(br => br.Book)
            .ThenInclude(b => b.Author)
            .Include(br => br.Book)
            .ThenInclude(b => b.Category)
            .Include(br => br.Member)
            .FirstOrDefaultAsync(br => br.Id == id && !br.IsDeleted);
    }

    public async Task<List<BorrowRecord>> GetAllAsync()
    {
        return await _context.BorrowRecords
            .Include(br => br.Book)
            .ThenInclude(b => b.Author)
            .Include(br => br.Book)
            .ThenInclude(b => b.Category)
            .Include(br => br.Member)
            .Where(br => !br.IsDeleted)
            .OrderByDescending(br => br.BorrowDate)
            .ToListAsync();
    }
    public async Task<BorrowRecord> UpdateAsync(BorrowRecord borrowRecord)
    {
        _context.BorrowRecords.Update(borrowRecord);
        await _context.SaveChangesAsync();
        return borrowRecord;
    }
    public async Task<BorrowRecord> DeleteAsync(BorrowRecord borrowRecord)
    {
        borrowRecord.IsDeleted = true;
        borrowRecord.DeletedOn = DateTimeOffset.Now;

        _context.BorrowRecords.Update(borrowRecord);
        await _context.SaveChangesAsync();

        return borrowRecord;
    }
    public async Task<List<BorrowRecord>> GetActiveBorrowsAsync()
    {
        return await _context.BorrowRecords
            .Include(br => br.Book)                    
            .ThenInclude(b => b.Author)                
            .Include(br => br.Book)                   
            .ThenInclude(b => b.Category)              
            .Include(br => br.Member)                  
            .Where(br => !br.IsDeleted && br.Status == BorrowStatus.Borrowed)
            .OrderBy(br => br.DueDate)                 
            .ToListAsync();
    }
    public async Task<List<BorrowRecord>> GetOverdueBorrowsAsync()
    {
        return await _context.BorrowRecords
            .Include(br => br.Book)                
            .ThenInclude(b => b.Author)              
            .Include(br => br.Book)                    
            .ThenInclude(b => b.Category)             
            .Include(br => br.Member)                 
            .Where(br => !br.IsDeleted &&
                        br.Status == BorrowStatus.Borrowed &&
                        br.DueDate < DateTime.Now)
            .OrderBy(br => br.DueDate)                
            .ToListAsync();
    }
    public async Task<List<BorrowRecord>> GetBorrowsByBookAsync(Guid bookId)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)                 
            .Where(br => br.BookId == bookId && !br.IsDeleted)
            .OrderByDescending(br => br.BorrowDate)   
            .ToListAsync();
    }
    public async Task<List<BorrowRecord>> GetBorrowsByMemberAsync(Guid memberId)
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
