using LibraryApp.Data.Context;
using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data.Repositories;

// Category entity'si için repository
public class CategoryRepository
{
    private readonly LibraryDbContext _context;

    public CategoryRepository(LibraryDbContext context)
    {
        _context = context;
    }

    // CRUD İşlemleri

    public async Task<Category> AddAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == name && !c.IsDeleted);
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> DeleteAsync(Category category)
    {
        category.IsDeleted = true;
        category.DeletedOn = DateTimeOffset.Now;
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    /// <summary>
    /// Kategorideki kitapları getir
    /// </summary>
    public async Task<List<Book>> GetBooksByCategoryAsync(Guid categoryId)
    {
        return await _context.Books
            .Where(b => b.CategoryId == categoryId && !b.IsDeleted)
            .ToListAsync();
    }
}