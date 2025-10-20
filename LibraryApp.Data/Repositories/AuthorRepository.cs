using LibraryApp.Data.Context;
using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApp.Data.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryDbContext _context;

    public AuthorRepository(LibraryDbContext context)
    {
        _context = context;
    }

    // IRepository<Author> - Base methods
    public async Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Author>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted)
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Author>> FindAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Where(predicate)
            .Where(a => !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<Author?> FirstOrDefaultAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Author, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted)
            .CountAsync(predicate, cancellationToken);
    }

    public async Task<(IEnumerable<Author> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<Author, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Authors.Where(a => !a.IsDeleted);

        if (predicate != null)
            query = query.Where(predicate);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Author> AddAsync(Author entity, CancellationToken cancellationToken = default)
    {
        await _context.Authors.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<Author>> AddRangeAsync(IEnumerable<Author> entities, CancellationToken cancellationToken = default)
    {
        await _context.Authors.AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public Task UpdateAsync(Author entity, CancellationToken cancellationToken = default)
    {
        _context.Authors.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Author entity, CancellationToken cancellationToken = default)
    {
        _context.Authors.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.Authors.Remove(entity);
        }
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTimeOffset.Now;
            _context.Authors.Update(entity);
        }
    }

    public Task UpdateRangeAsync(IEnumerable<Author> entities, CancellationToken cancellationToken = default)
    {
        _context.Authors.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<Author> entities, CancellationToken cancellationToken = default)
    {
        _context.Authors.RemoveRange(entities);
        return Task.CompletedTask;
    }

    // IAuthorRepository - Author-specific methods
    public async Task<IEnumerable<Author>> GetAuthorsByNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted &&
                       a.FirstName.Contains(firstName) &&
                       a.LastName.Contains(lastName))
            .ToListAsync(cancellationToken);
    }

    public async Task<Author?> GetAuthorByNameAsync(string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => !a.IsDeleted &&
                                     a.FirstName == firstName &&
                                     a.LastName == lastName, cancellationToken);
    }

    public async Task<IEnumerable<Author>> SearchAuthorsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted &&
                       (a.FirstName.Contains(searchTerm) ||
                        a.LastName.Contains(searchTerm) ||
                        (a.Biography != null && a.Biography.Contains(searchTerm))))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Author>> GetAuthorsWithBooksAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Include(a => a.Books)
            .Where(a => !a.IsDeleted && a.Books.Any(b => !b.IsDeleted))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Author>> GetAuthorsByBirthYearAsync(int birthYear, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted &&
                       a.BirthDate.HasValue &&
                       a.BirthDate.Value.Year == birthYear)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetAuthorCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .CountAsync(a => !a.IsDeleted, cancellationToken);
    }

    public async Task<int> GetAuthorsWithBooksCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted && a.Books.Any(b => !b.IsDeleted))
            .CountAsync(cancellationToken);
    }

    public async Task<bool> IsAuthorNameUniqueAsync(string firstName, string lastName, Guid? excludeAuthorId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Authors
            .Where(a => !a.IsDeleted &&
                       a.FirstName == firstName &&
                       a.LastName == lastName);

        if (excludeAuthorId.HasValue)
            query = query.Where(a => a.Id != excludeAuthorId.Value);

        return !await query.AnyAsync(cancellationToken);
    }
}
