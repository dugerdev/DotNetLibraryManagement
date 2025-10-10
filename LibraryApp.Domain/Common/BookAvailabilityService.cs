using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Domain.Common;

/// <summary>
/// Kitap müsaitlik işlemlerini yöneten domain service implementasyonu
/// Bu sınıf, kitap müsaitlik durumu ile ilgili tüm business logic'i içerir
/// </summary>
public class BookAvailabilityService : IBookAvailabilityService
{
    private readonly IBookRepository _bookRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    /// <summary>
    /// BookAvailabilityService constructor'ı
    /// </summary>
    /// <param name="bookRepository">Kitap repository'si</param>
    /// <param name="borrowRecordRepository">Ödünç verme kaydı repository'si</param>
    public BookAvailabilityService(
        IBookRepository bookRepository,
        IBorrowRecordRepository borrowRecordRepository)
    {
        _bookRepository = bookRepository;
        _borrowRecordRepository = borrowRecordRepository;
    }

    /// <summary>
    /// Kitabın müsait olup olmadığını kontrol eder
    /// Bu method, kitabın ödünç alınabilir durumda olup olmadığını kontrol eder
    /// </summary>
    public async Task<bool> IsBookAvailableAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        // 1. Kitabı bul
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);
        if (book == null)
            return false; // Kitap bulunamadı

        // 2. Domain entity'deki business rule'u kullan
        return book.CanBeBorrowed;
    }

    /// <summary>
    /// Kitabın müsait kopya sayısını getirir
    /// </summary>
    public async Task<int> GetAvailableCopiesCountAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        // 1. Kitabı bul
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);
        if (book == null)
            return 0; // Kitap bulunamadı

        // 2. Müsait kopya sayısını döndür
        return book.AvailableCopies;
    }

    /// <summary>
    /// Kitabın toplam kopya sayısını getirir
    /// </summary>
    public async Task<int> GetTotalCopiesCountAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        // 1. Kitabı bul
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);
        if (book == null)
            return 0; // Kitap bulunamadı

        // 2. Toplam kopya sayısını döndür
        return book.TotalCopies;
    }

    /// <summary>
    /// Kitabın ödünç alınan kopya sayısını getirir
    /// Bu method, aktif ödünç verme kayıtlarını sayar
    /// </summary>
    public async Task<int> GetBorrowedCopiesCountAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        // 1. Kitabın aktif ödünç verme kayıtlarını getir
        var activeBorrowRecords = await _borrowRecordRepository.GetBorrowRecordsByBookAsync(bookId, cancellationToken);
        
        // 2. Sadece aktif (Borrowed) kayıtları say
        var borrowedCount = activeBorrowRecords.Count(r => r.Status == Enums.BorrowStatus.Borrowed);
        
        return borrowedCount;
    }

    /// <summary>
    /// Kitabın müsaitlik durumunu günceller
    /// Bu method, ödünç verme/iade işlemlerinde kullanılır
    /// </summary>
    public async Task UpdateAvailabilityAsync(Guid bookId, int newAvailableCount, CancellationToken cancellationToken = default)
    {
        // 1. Kitabı bul
        var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);
        if (book == null)
            throw new EntityNotFoundException("Book", bookId);

        // 2. Yeni müsait kopya sayısını set et
        book.AvailableCopies = newAvailableCount;

        // 3. Business rule kontrolü: Müsait sayı negatif olamaz
        if (book.AvailableCopies < 0)
            throw new Exceptions.InvalidOperationException("Available copies cannot be negative");

        // 4. Business rule kontrolü: Müsait sayı toplam sayıdan fazla olamaz
        if (book.AvailableCopies > book.TotalCopies)
            throw new Exceptions.InvalidOperationException("Available copies cannot exceed total copies");

        // 5. Kitabı güncelle
        await _bookRepository.UpdateAsync(book, cancellationToken);
    }
}
