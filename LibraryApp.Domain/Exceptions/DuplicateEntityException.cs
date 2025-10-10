namespace LibraryApp.Domain.Exceptions;

/// <summary>
/// Duplicate entity oluşturulmaya çalışıldığında fırlatılan exception
/// Bu exception şu durumlarda kullanılır:
/// - Aynı ISBN'li kitap eklenmeye çalışıldığında
/// - Aynı email'li üye eklenmeye çalışıldığında
/// - Aynı telefon numaralı üye eklenmeye çalışıldığında
/// - Aynı ad-soyadlı yazar eklenmeye çalışıldığında
/// 
/// Bu exception, benzersizlik kısıtlaması ihlal edildiğinde fırlatılır
/// </summary>
public class DuplicateEntityException : DomainException
{
    /// <summary>
    /// Duplicate entity exception'ı oluşturur
    /// </summary>
    /// <param name="entityName">Entity adı (örn: "Book", "Member")</param>
    /// <param name="fieldName">Benzersiz alan adı (örn: "ISBN", "Email")</param>
    /// <param name="value">Benzersiz değer</param>
    public DuplicateEntityException(string entityName, string fieldName, string value) 
        : base($"{entityName} with {fieldName} '{value}' already exists.")
    {
    }

    /// <summary>
    /// Duplicate entity exception'ı özel mesaj ile oluşturur
    /// </summary>
    /// <param name="message">Özel hata mesajı</param>
    public DuplicateEntityException(string message) 
        : base(message)
    {
    }
}
