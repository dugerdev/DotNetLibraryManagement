using LibraryApp.Application.DTOs.Categories;
using LibraryApp.Application.Interfaces;
using LibraryApp.Application.Mappers;
using LibraryApp.Domain.Common;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Application.Services;

/// <summary>
/// Kategori application service implementasyonu
/// </summary>
public class CategoryApplicationService : ICategoryApplicationService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryApplicationService(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateCategoryAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        // Kategori adı benzersizlik kontrolü
        var existingCategory = await _categoryRepository.GetCategoryByNameAsync(dto.Name, cancellationToken);
        if (existingCategory != null)
            throw new DuplicateEntityException("Category", "Name", dto.Name);

        // Entity oluştur
        var category = CategoryMapper.ToEntity(dto);
        var createdCategory = await _categoryRepository.AddAsync(category, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return createdCategory.Id;
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        return category != null ? CategoryMapper.ToDto(category) : null;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.Select(CategoryMapper.ToDto);
    }

    public async Task<bool> UpdateCategoryAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
            return false;

        // Kategori adı benzersizlik kontrolü (kendisi hariç)
        var existingCategory = await _categoryRepository.GetCategoryByNameAsync(dto.Name, cancellationToken);
        if (existingCategory != null && existingCategory.Id != id)
            throw new DuplicateEntityException("Category", "Name", dto.Name);

        // Güncelle
        CategoryMapper.UpdateEntity(category, dto);
        await _categoryRepository.UpdateAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category == null)
            return false;

        await _categoryRepository.SoftDeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<CategoryDto>> SearchCategoriesAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.SearchCategoriesAsync(searchTerm, cancellationToken);
        return categories.Select(CategoryMapper.ToDto);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesWithBooksAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetCategoriesWithBooksAsync(cancellationToken);
        return categories.Select(CategoryMapper.ToDto);
    }
}
