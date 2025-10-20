using LibraryApp.Data.Context;
using LibraryApp.Domain.Common;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApp.Data.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly LibraryDbContext _context;

    public CategoryRepository(LibraryDbContext context)
    {
        _context = context;
    }

    // IRepository<Category> - Base methods
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> FindAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(predicate)
            .Where(c => !c.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> FirstOrDefaultAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted)
            .AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted)
            .CountAsync(predicate, cancellationToken);
    }

    public async Task<(IEnumerable<Category> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<Category, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Categories.Where(c => !c.IsDeleted);

        if (predicate != null)
            query = query.Where(predicate);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Category> AddAsync(Category entity, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<Category>> AddRangeAsync(IEnumerable<Category> entities, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        _context.Categories.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Category entity, CancellationToken cancellationToken = default)
    {
        _context.Categories.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _context.Categories.Remove(entity);
        }
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTimeOffset.Now;
            _context.Categories.Update(entity);
        }
    }

    public Task UpdateRangeAsync(IEnumerable<Category> entities, CancellationToken cancellationToken = default)
    {
        _context.Categories.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<Category> entities, CancellationToken cancellationToken = default)
    {
        _context.Categories.RemoveRange(entities);
        return Task.CompletedTask;
    }

    // ICategoryRepository - Category-specific methods
    public async Task<Category?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => !c.IsDeleted && c.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted &&
                       (c.Name.Contains(searchTerm) ||
                        (c.Description != null && c.Description.Contains(searchTerm))))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithBooksAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Include(c => c.Books)
            .Where(c => !c.IsDeleted && c.Books.Any(b => !b.IsDeleted))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetEmptyCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Include(c => c.Books)
            .Where(c => !c.IsDeleted && !c.Books.Any(b => !b.IsDeleted))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCategoryCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .CountAsync(c => !c.IsDeleted, cancellationToken);
    }

    public async Task<int> GetCategoriesWithBooksCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted && c.Books.Any(b => !b.IsDeleted))
            .CountAsync(cancellationToken);
    }

    public async Task<bool> IsCategoryNameUniqueAsync(string name, Guid? excludeCategoryId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Categories
            .Where(c => !c.IsDeleted && c.Name == name);

        if (excludeCategoryId.HasValue)
            query = query.Where(c => c.Id != excludeCategoryId.Value);

        return !await query.AnyAsync(cancellationToken);
    }
}
