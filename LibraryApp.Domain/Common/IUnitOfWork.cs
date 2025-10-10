using LibraryApp.Domain.Common;

namespace LibraryApp.Domain.Common;

/// <summary>
/// Unit of Work pattern interface'i
/// Transaction yönetimi ve repository'lerin koordinasyonu için kullanılır
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // Repository'ler
    IAuthorRepository Authors { get; }
    IBookRepository Books { get; }
    ICategoryRepository Categories { get; }
    IMemberRepository Members { get; }
    IBorrowRecordRepository BorrowRecords { get; }

    // Transaction işlemleri
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}