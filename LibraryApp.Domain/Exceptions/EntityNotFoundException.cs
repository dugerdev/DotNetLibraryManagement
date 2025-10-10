namespace LibraryApp.Domain.Exceptions;

/// <summary>
/// Entity bulunamadığında fırlatılan exception
/// Bu exception şu durumlarda kullanılır:
/// - Repository'de entity bulunamadığında
/// - ID'ye göre arama yapıldığında sonuç bulunamadığında
/// - Soft delete edilmiş entity'ye erişim yapıldığında
/// 
/// Bu exception, veri erişim işlemlerinde entity bulunamadığında fırlatılır
/// </summary>
public class EntityNotFoundException : DomainException
{
    /// <summary>
    /// Entity bulunamadı exception'ı ID ile oluşturur
    /// </summary>
    /// <param name="entityName">Entity adı (örn: "Book", "Author")</param>
    /// <param name="id">Aranan ID</param>
    public EntityNotFoundException(string entityName, Guid id) 
        : base($"{entityName} with ID '{id}' was not found.")
    {
    }

    /// <summary>
    /// Entity bulunamadı exception'ı identifier ile oluşturur
    /// </summary>
    /// <param name="entityName">Entity adı</param>
    /// <param name="identifier">Aranan identifier (örn: ISBN, Email)</param>
    public EntityNotFoundException(string entityName, string identifier) 
        : base($"{entityName} with identifier '{identifier}' was not found.")
    {
    }

    /// <summary>
    /// Entity bulunamadı exception'ı özel mesaj ile oluşturur
    /// </summary>
    /// <param name="message">Özel hata mesajı</param>
    public EntityNotFoundException(string message) 
        : base(message)
    {
    }
}
