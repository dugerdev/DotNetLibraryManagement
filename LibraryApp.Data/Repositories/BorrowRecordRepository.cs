using LibraryApp.Data.Context;
using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApp.Data.Repositories;

public class BorrowRecordRepository : IBorrowRecordRepository
{
    private readonly LibraryDbContext _context;

    public BorrowRecordRepository(LibraryDbContext context)
    {
        _context = context;
    }

    // IRepository<BorrowRecord> - Base methods
    public async Task<BorrowRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
                .ThenInclude(b => b.Author)
            .Include(br => br.Book)
                .ThenInclude(b => b.Category)
            .FirstOrDefaultAsync(br => br.Id == id && !br.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<BorrowRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
                .ThenInclude(b => b.Author)
            .Include(br => br.Book)
                .ThenInclude(b => b.Category)
            .Where(br => !br.IsDeleted)
            .OrderByDescending(br => br.BorrowDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BorrowRecord>> FindAsync(Expression<Func<BorrowRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
            .Where(predicate)
            .Where(br => !br.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<BorrowRecord?> FirstOrDefaultAsync(Expression<Func<BorrowRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
            .Where(br => !br.IsDeleted)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<BorrowRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Where(br => !br.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<BorrowRecord, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Where(br => !br.IsDeleted)
            .CountAsync(predicate, cancellationToken);
    }

    public async Task<(IEnumerable<BorrowRecord> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<BorrowRecord, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
            .Where(br => !br.IsDeleted);

        if (predicate != null)
            query = query.Where(predicate);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(br => br.BorrowDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<BorrowRecord> AddAsync(BorrowRecord entity, CancellationToken cancellationToken = default)
    {
        await _context.BorrowRecords.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<BorrowRecord>> AddRangeAsync(IEnumerable<BorrowRecord> entities, CancellationToken cancellationToken = default)
    {
        await _context.BorrowRecords.AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public Task UpdateAsync(BorrowRecord entity, CancellationToken cancellationToken = default)
    {
        _context.BorrowRecords.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(BorrowRecord entity, CancellationToken cancellationToken = default)
    {
        _context.BorrowRecords.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.BorrowRecords.Remove(entity);
        }
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTimeOffset.Now;
            _context.BorrowRecords.Update(entity);
        }
    }

    public Task UpdateRangeAsync(IEnumerable<BorrowRecord> entities, CancellationToken cancellationToken = default)
    {
        _context.BorrowRecords.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<BorrowRecord> entities, CancellationToken cancellationToken = default)
    {
        _context.BorrowRecords.RemoveRange(entities);
        return Task.CompletedTask;
    }

    // IBorrowRecordRepository - BorrowRecord-specific methods
    public async Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByMemberAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
                .ThenInclude(b => b.Author)
            .Where(br => !br.IsDeleted && br.MemberId == memberId)
            .OrderByDescending(br => br.BorrowDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByBookAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
            .Where(br => !br.IsDeleted && br.BookId == bookId)
            .OrderByDescending(br => br.BorrowDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BorrowRecord>> GetActiveBorrowRecordsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
                .ThenInclude(b => b.Author)
            .Where(br => !br.IsDeleted && br.Status == BorrowStatus.Borrowed)
            .OrderByDescending(br => br.BorrowDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
                .ThenInclude(b => b.Author)
            .Where(br => !br.IsDeleted &&
                        br.Status == BorrowStatus.Borrowed &&
                        br.DueDate < DateTimeOffset.Now)
            .OrderBy(br => br.DueDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BorrowRecord>> GetBorrowRecordsByStatusAsync(BorrowStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
                .ThenInclude(b => b.Author)
            .Where(br => !br.IsDeleted && br.Status == status)
            .OrderByDescending(br => br.BorrowDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<BorrowRecord?> GetActiveBorrowRecordAsync(Guid memberId, Guid bookId, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .Include(br => br.Member)
            .Include(br => br.Book)
            .FirstOrDefaultAsync(br => !br.IsDeleted &&
                                      br.MemberId == memberId &&
                                      br.BookId == bookId &&
                                      br.Status == BorrowStatus.Borrowed, cancellationToken);
    }

    public async Task<int> GetActiveBorrowCountAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .CountAsync(br => !br.IsDeleted &&
                             br.MemberId == memberId &&
                             br.Status == BorrowStatus.Borrowed, cancellationToken);
    }

    public async Task<int> GetTotalBorrowCountAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .CountAsync(br => !br.IsDeleted && br.MemberId == memberId, cancellationToken);
    }

    public async Task<decimal> GetTotalFineAmountAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        var records = await _context.BorrowRecords
            .Where(br => !br.IsDeleted &&
                        br.MemberId == memberId &&
                        br.FineAmount.HasValue &&
                        br.FineAmount > 0)
            .ToListAsync(cancellationToken);

        return records.Sum(br => br.FineAmount ?? 0);
    }

    public async Task<bool> CanMemberBorrowAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        // Üyenin aktif ödünç kitap sayısını kontrol et (maksimum 5)
        var activeBorrowCount = await GetActiveBorrowCountAsync(memberId, cancellationToken);
        if (activeBorrowCount >= 5)
            return false;

        // Gecikmiş kitabı var mı kontrol et
        var hasOverdue = await HasOverdueBooksAsync(memberId, cancellationToken);
        if (hasOverdue)
            return false;

        return true;
    }

    public async Task<bool> HasOverdueBooksAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.BorrowRecords
            .AnyAsync(br => !br.IsDeleted &&
                           br.MemberId == memberId &&
                           br.Status == BorrowStatus.Borrowed &&
                           br.DueDate < DateTimeOffset.Now, cancellationToken);
    }
}
