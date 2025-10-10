namespace LibraryApp.Domain.Exceptions;

/// <summary>
/// Domain katmanı için temel exception sınıfı
/// Tüm domain-specific exception'lar bu sınıftan türer
/// Bu sınıf, domain katmanındaki business rule ihlallerini temsil eder
/// 
/// Bu sınıfın amacı:
/// 1. Domain exception'larını diğer exception'lardan ayırmak
/// 2. Domain-specific error handling sağlamak
/// 3. Exception hierarchy oluşturmak
/// 4. Business rule violation'larını temsil etmek
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// Domain exception'ı oluşturur
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    protected DomainException(string message) : base(message)
    {
    }

    /// <summary>
    /// Domain exception'ı inner exception ile oluşturur
    /// </summary>
    /// <param name="message">Hata mesajı</param>
    /// <param name="innerException">İç hata</param>
    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
