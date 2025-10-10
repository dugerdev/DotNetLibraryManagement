using LibraryApp.Domain.Entities;

namespace LibraryApp.Domain.Common;

/// <summary>
/// Kitap ödünç verme işlemlerini yöneten domain service interface'i
/// Bu service, kitap ödünç verme ile ilgili tüm business logic'i içerir
/// Clean Architecture'da domain service'ler, birden fazla repository'yi koordine eder
/// ve karmaşık business rule'ları implement eder
/// </summary>
public interface IBorrowingService
{
    /// <summary>
    /// Üyenin belirli bir kitabı ödünç alıp alamayacağını kontrol eder
    /// Bu method şu kontrolleri yapar:
    /// - Üye aktif mi?
    /// - Üyelik geçerli mi?
    /// - Maksimum ödünç alma limiti aşılmış mı?
    /// - Kitap müsait mi?
    /// </summary>
    /// <param name="memberId">Üye ID'si</param>
    /// <param name="bookId">Kitap ID'si</param>
    /// <param name="cancellationToken">İşlem iptal token'ı</param>
    /// <returns>True eğer üye kitabı ödünç alabilirse</returns>
    Task<bool> CanMemberBorrowBookAsync(Guid memberId, Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Üyeye kitap ödünç verir
    /// Bu method şu işlemleri yapar:
    /// - Ödünç alma koşullarını kontrol eder
    /// - Yeni BorrowRecord oluşturur
    /// - Kitabın müsait kopya sayısını azaltır
    /// - Veritabanına kaydeder
    /// </summary>
    /// <param name="memberId">Üye ID'si</param>
    /// <param name="bookId">Kitap ID'si</param>
    /// <param name="cancellationToken">İşlem iptal token'ı</param>
    /// <returns>Oluşturulan ödünç verme kaydı</returns>
    /// <exception cref="MemberCannotBorrowException">Üye ödünç alamazsa</exception>
    /// <exception cref="BookNotAvailableException">Kitap müsait değilse</exception>
    Task<BorrowRecord> BorrowBookAsync(Guid memberId, Guid bookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ödünç verilen kitabı geri alır
    /// Bu method şu işlemleri yapar:
    /// - BorrowRecord'u bulur
    /// - Geri verme tarihini set eder
    /// - Status'u Returned yapar
    /// - Kitabın müsait kopya sayısını artırır
    /// </summary>
    /// <param name="borrowRecordId">Ödünç verme kaydı ID'si</param>
    /// <param name="cancellationToken">İşlem iptal token'ı</param>
    /// <exception cref="EntityNotFoundException">Kayıt bulunamazsa</exception>
    Task ReturnBookAsync(Guid borrowRecordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ödünç verme kaydı için ceza miktarını hesaplar
    /// Bu method şu hesaplamaları yapar:
    /// - Kitap süresi geçmiş mi?
    /// - Kaç gün gecikmiş?
    /// - Günlük ceza miktarı nedir?
    /// </summary>
    /// <param name="borrowRecordId">Ödünç verme kaydı ID'si</param>
    /// <param name="cancellationToken">İşlem iptal token'ı</param>
    /// <returns>Hesaplanan ceza miktarı (TL)</returns>
    Task<decimal> CalculateFineAsync(Guid borrowRecordId, CancellationToken cancellationToken = default);
}
