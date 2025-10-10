namespace LibraryApp.Domain.Exceptions;

/// <summary>
/// Kitap ödünç alınamadığında fırlatılan exception
/// Bu exception şu durumlarda kullanılır:
/// - Kitap müsait değilse (AvailableCopies = 0)
/// - Kitap silinmişse (IsDeleted = true)
/// - Kitap başka bir üye tarafından ödünç alınmışsa
/// 
/// Bu exception, kitap ödünç alma işleminde business rule ihlali olduğunda fırlatılır
/// </summary>
public class BookNotAvailableException : DomainException
{
    /// <summary>
    /// Kitap müsait değil exception'ı oluşturur
    /// </summary>
    /// <param name="bookTitle">Kitap başlığı</param>
    public BookNotAvailableException(string bookTitle) 
        : base($"Book '{bookTitle}' is not available for borrowing.")
    {
    }

    /// <summary>
    /// Kitap müsait değil exception'ı detaylı sebep ile oluşturur
    /// </summary>
    /// <param name="bookTitle">Kitap başlığı</param>
    /// <param name="reason">Müsait olmama sebebi</param>
    public BookNotAvailableException(string bookTitle, string reason) 
        : base($"Book '{bookTitle}' is not available for borrowing. Reason: {reason}")
    {
    }
}
