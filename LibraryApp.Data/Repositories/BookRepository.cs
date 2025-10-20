using LibraryApp.Data.Context;
using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApp.Data.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    // IRepository<Book> - Base methods
    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted)
            .OrderBy(b => b.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Book>> FindAsync(Expression<Func<Book, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(predicate)
            .Where(b => !b.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<Book?> FirstOrDefaultAsync(Expression<Func<Book, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<Book, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Where(b => !b.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Book, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Where(b => !b.IsDeleted)
            .CountAsync(predicate, cancellationToken);
    }

    public async Task<(IEnumerable<Book> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<Book, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted);

        if (predicate != null)
            query = query.Where(predicate);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(b => b.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Book> AddAsync(Book entity, CancellationToken cancellationToken = default)
    {
        await _context.Books.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<Book>> AddRangeAsync(IEnumerable<Book> entities, CancellationToken cancellationToken = default)
    {
        await _context.Books.AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public Task UpdateAsync(Book entity, CancellationToken cancellationToken = default)
    {
        _context.Books.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Book entity, CancellationToken cancellationToken = default)
    {
        _context.Books.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.Books.Remove(entity);
        }
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTimeOffset.Now;
            _context.Books.Update(entity);
        }
    }

    public Task UpdateRangeAsync(IEnumerable<Book> entities, CancellationToken cancellationToken = default)
    {
        _context.Books.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<Book> entities, CancellationToken cancellationToken = default)
    {
        _context.Books.RemoveRange(entities);
        return Task.CompletedTask;
    }

    // IBookRepository - Book-specific methods
    public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted && b.AuthorId == authorId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted && b.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted && b.AvailableCopies > 0)
            .OrderBy(b => b.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted &&
                       (b.Title.Contains(searchTerm) ||
                        b.ISBN.Contains(searchTerm) ||
                        b.Author.FirstName.Contains(searchTerm) ||
                        b.Author.LastName.Contains(searchTerm)))
            .ToListAsync(cancellationToken);
    }

    public async Task<Book?> GetBookByIsbnAsync(string isbn, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => !b.IsDeleted && b.ISBN == isbn, cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetOverdueBooksAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Include(b => b.BorrowRecords)
            .Where(b => !b.IsDeleted &&
                       b.BorrowRecords.Any(br => !br.IsDeleted &&
                                                 br.Status == Domain.Enums.BorrowStatus.Borrowed &&
                                                 br.DueDate < DateTimeOffset.Now))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetPopularBooksAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Include(b => b.BorrowRecords)
            .Where(b => !b.IsDeleted)
            .OrderByDescending(b => b.BorrowRecords.Count(br => !br.IsDeleted))
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetBooksByPublishedYearAsync(int year, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted && b.PublishedDate.Year == year)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsBookAvailableAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        var book = await GetByIdAsync(bookId, cancellationToken);
        return book != null && book.AvailableCopies > 0;
    }

    public async Task<int> GetAvailableCopiesCountAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        var book = await GetByIdAsync(bookId, cancellationToken);
        return book?.AvailableCopies ?? 0;
    }

    public async Task UpdateAvailableCopiesAsync(Guid bookId, int newCount, CancellationToken cancellationToken = default)
    {
        var book = await GetByIdAsync(bookId, cancellationToken);
        if (book != null)
        {
            book.AvailableCopies = newCount;
            _context.Books.Update(book);
        }
    }

    public async Task<bool> IsIsbnUniqueAsync(string isbn, Guid? excludeBookId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Books
            .Where(b => !b.IsDeleted && b.ISBN == isbn);

        if (excludeBookId.HasValue)
            query = query.Where(b => b.Id != excludeBookId.Value);

        return !await query.AnyAsync(cancellationToken);
    }

    public async Task<int> GetTotalBookCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .CountAsync(b => !b.IsDeleted, cancellationToken);
    }

    public async Task<int> GetAvailableBookCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .CountAsync(b => !b.IsDeleted && b.AvailableCopies > 0, cancellationToken);
    }
}
