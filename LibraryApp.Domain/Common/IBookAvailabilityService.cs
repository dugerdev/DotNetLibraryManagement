using LibraryApp.Domain.Entities;

namespace LibraryApp.Domain.Common;

/// <summary>
/// Kitap müsaitlik işlemlerini yöneten domain service interface'i
/// Bu service, kitap müsaitlik durumu ile ilgili business logic'i içerir
/// </summary>
public interface IBookAvailabilityService
{
    /// <summary>
    /// Kitabın müsait olup olmadığını kontrol eder
    /// </summary>
    /// <param name="bookId">Kitap ID'si</param>
    /// <param name="cancellationToken">İşlem iptal token'ı</param>
    /// <returns>True eğer kitap müsaitse</returns>
    Task<bool> IsBookAvailableAsync(Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kitabın müsait kopya sayısını getirir
    /// </summary>
    /// <param name="bookId">Kitap ID'si</param>
    /// <param name="cancellationToken">İşlem iptal token'ı</param>
    /// <returns>Müsait kopya sayısı</returns>
    Task<int> GetAvailableCopiesCountAsync(Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kitabın toplam kopya sayısını getirir
    /// </summary>
    /// <param name="bookId">Kitap ID'si</param>
    /// <param name="cancellationToken">İşlem iptal token'ı</param>
    /// <returns>Toplam kopya sayısı</returns>
    Task<int> GetTotalCopiesCountAsync(Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kitabın ödünç alınan kopya sayısını getirir
    /// </summary>
    /// <param name="bookId">Kitap ID'si</param>
    /// <param name="cancellationToken">İşlem iptal token'ı</param>
    /// <returns>Ödünç alınan kopya sayısı</returns>
    Task<int> GetBorrowedCopiesCountAsync(Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kitabın müsaitlik durumunu günceller
    /// </summary>
    /// <param name="bookId">Kitap ID'si</param>
    /// <param name="newAvailableCount">Yeni müsait kopya sayısı</param>
    /// <param name="cancellationToken">İşlem iptal token'ı</param>
    Task UpdateAvailabilityAsync(Guid bookId, int newAvailableCount, CancellationToken cancellationToken = default);
}
