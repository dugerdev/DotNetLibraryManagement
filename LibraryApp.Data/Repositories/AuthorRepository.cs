using LibraryApp.Data.Context;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data.Repositories;

public class AuthorRepository
{
    private readonly LibraryDbContext _context;

    public AuthorRepository(LibraryDbContext context)
    {
        _context = context;
    }

    //CRUD Islemleri 

    public async Task<Author> AddAsync(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Author?> GetByIdAsync(Guid id)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.Id != id && !a.IsDeleted);
    }

    public async Task<Author?> GetByNameAsync(string firstName,string lastName)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName && !a.IsDeleted);
    }

    public async Task<List<Author>> GetAllAsync()
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted)
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ToListAsync();
    }

    public async Task<Author> UpdateAsync(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Author> DeleteAsync(Author author)
    {
        author.IsDeleted = true;
        author.DeletedOn = DateTimeOffset.Now;
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
        return author;
    }
    public async Task<List<Book>> GetBooksByAuthorAsync(Guid authorId)
    {
        return await _context.Books
            .Where(b => b.AuthorId == authorId && !b.IsDeleted)
            .ToListAsync();
    }
}
