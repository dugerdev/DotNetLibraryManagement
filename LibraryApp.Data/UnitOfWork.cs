using LibraryApp.Data.Context;
using LibraryApp.Data.Repositories;
using LibraryApp.Domain.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryApp.Data;

/// <summary>
/// Unit of Work pattern implementasyonu
/// Transaction yönetimi ve repository koordinasyonu
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext _context;
    private IDbContextTransaction? _transaction;

    // Repository'ler - Lazy initialization
    private IAuthorRepository? _authors;
    private IBookRepository? _books;
    private ICategoryRepository? _categories;
    private IMemberRepository? _members;
    private IBorrowRecordRepository? _borrowRecords;

    public UnitOfWork(LibraryDbContext context)
    {
        _context = context;
    }

    // Repository property'leri - İlk erişimde oluşturulur
    public IAuthorRepository Authors => 
        _authors ??= new AuthorRepository(_context);

    public IBookRepository Books => 
        _books ??= new BookRepository(_context);

    public ICategoryRepository Categories => 
        _categories ??= new CategoryRepository(_context);

    public IMemberRepository Members => 
        _members ??= new MemberRepository(_context);

    public IBorrowRecordRepository BorrowRecords => 
        _borrowRecords ??= new BorrowRecordRepository(_context);

    // Transaction işlemleri
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    // Dispose pattern
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

