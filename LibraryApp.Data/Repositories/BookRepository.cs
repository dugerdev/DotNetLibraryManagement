using LibraryApp.Data.Context;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data.Repositories;

public class BookRepository
{
    private readonly LibraryDbContext _context;
    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    //CRUD ISLEMLERI 

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _context.Books
            .Include(b => b.Author)      // Yazar bilgisini de getir
            .Include(b => b.Category)    // Kategori bilgisini de getir
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.ISBN == isbn && !b.IsDeleted);
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<List<Book>> GetAvailableBooksAsync()
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted && b.AvailableCopies > 0)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _context.Books.Update(book);
            await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> DeleteAsync (Book book)
    {
        book.IsDeleted = true;
        book.DeletedOn = DateTimeOffset.Now;

        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<List<Book>> GetBooksByAuthorAsync(Guid authorId)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => b.AuthorId == authorId && !b.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<Book>> GetBooksByCategoryAsync(Guid categoryId)
    {
        return await _context.Books
            .Include(b => b.Category)
            .Include(b => b.Author)
            .Where(b => b.CategoryId == categoryId && !b.IsDeleted)
            .ToListAsync();
    }
}
