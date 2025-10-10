namespace LibraryApp.Domain.Exceptions;

/// <summary>
/// Geçersiz işlem yapıldığında fırlatılan exception
/// Bu exception şu durumlarda kullanılır:
/// - Business rule'ları ihlal edildiğinde
/// - Geçersiz state transition yapıldığında
/// - Mantıksal olarak yanlış işlem yapıldığında
/// 
/// Bu exception, domain business rule'ları ihlal edildiğinde fırlatılır
/// </summary>
public class InvalidOperationException : DomainException
{
    /// <summary>
    /// Geçersiz işlem exception'ı oluşturur
    /// </summary>
    /// <param name="operation">Yapılmaya çalışılan işlem</param>
    /// <param name="reason">İşlemin geçersiz olma sebebi</param>
    public InvalidOperationException(string operation, string reason) 
        : base($"Invalid operation '{operation}': {reason}")
    {
    }

    /// <summary>
    /// Geçersiz işlem exception'ı özel mesaj ile oluşturur
    /// </summary>
    /// <param name="message">Özel hata mesajı</param>
    public InvalidOperationException(string message) 
        : base(message)
    {
    }
}
