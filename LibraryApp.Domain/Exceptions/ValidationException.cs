namespace LibraryApp.Domain.Exceptions;

/// <summary>
/// Validation hatalarında fırlatılan exception
/// Bu exception şu durumlarda kullanılır:
/// - Entity validation'ları başarısız olduğunda
/// - Input validation'ları başarısız olduğunda
/// - Business rule validation'ları başarısız olduğunda
/// 
/// Bu exception, domain validation kuralları ihlal edildiğinde fırlatılır
/// </summary>
public class ValidationException : DomainException
{
    /// <summary>
    /// Validation exception'ı alan adı ile oluşturur
    /// </summary>
    /// <param name="fieldName">Hatalı alan adı</param>
    /// <param name="message">Validation hata mesajı</param>
    public ValidationException(string fieldName, string message) 
        : base($"Validation error for field '{fieldName}': {message}")
    {
    }

    /// <summary>
    /// Validation exception'ı özel mesaj ile oluşturur
    /// </summary>
    /// <param name="message">Validation hata mesajı</param>
    public ValidationException(string message) 
        : base($"Validation error: {message}")
    {
    }

    /// <summary>
    /// Validation exception'ı inner exception ile oluşturur
    /// </summary>
    /// <param name="message">Validation hata mesajı</param>
    /// <param name="innerException">İç hata</param>
    public ValidationException(string message, Exception innerException) 
        : base($"Validation error: {message}", innerException)
    {
    }
}
