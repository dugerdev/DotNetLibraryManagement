namespace LibraryApp.Domain.Exceptions;

/// <summary>
/// Üye kitap ödünç alamadığında fırlatılan exception
/// Bu exception şu durumlarda kullanılır:
/// - Üyelik süresi dolmuşsa
/// - Üye aktif değilse
/// - Maksimum ödünç alma limiti aşılmışsa
/// - Üyenin süresi geçmiş kitapları varsa
/// 
/// Bu exception, üye ödünç alma işleminde business rule ihlali olduğunda fırlatılır
/// </summary>
public class MemberCannotBorrowException : DomainException
{
    /// <summary>
    /// Üye ödünç alamaz exception'ı oluşturur
    /// </summary>
    /// <param name="reason">Ödünç alamama sebebi</param>
    public MemberCannotBorrowException(string reason) 
        : base($"Member cannot borrow books: {reason}")
    {
    }

    /// <summary>
    /// Üye ödünç alamaz exception'ı mevcut ve maksimum sayı ile oluşturur
    /// </summary>
    /// <param name="reason">Ödünç alamama sebebi</param>
    /// <param name="currentBorrowCount">Mevcut ödünç kitap sayısı</param>
    /// <param name="maxAllowed">Maksimum izin verilen sayı</param>
    public MemberCannotBorrowException(string reason, int currentBorrowCount, int maxAllowed) 
        : base($"Member cannot borrow books: {reason}. Current borrow count: {currentBorrowCount}, Max allowed: {maxAllowed}")
    {
    }
}
