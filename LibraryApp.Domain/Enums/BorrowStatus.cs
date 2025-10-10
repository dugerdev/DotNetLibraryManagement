namespace LibraryApp.Domain.Enums
{
    /// <summary>
    /// Ödünç verme durumu enum'u
    /// Bu enum, bir kitabın ödünç verme sürecindeki durumunu temsil eder
    /// 
    /// Enum kullanmanın avantajları:
    /// 1. Type safety: Sadece geçerli değerler kullanılabilir
    /// 2. IntelliSense desteği: IDE'de otomatik tamamlama
    /// 3. Refactoring güvenliği: Değer değişiklikleri kolayca yapılabilir
    /// 4. Veritabanında sayısal değer olarak saklanır (performans)
    /// 
    /// Not: Enum değerleri 1'den başlar, 0 değil
    /// Bu sayede veritabanında 0 değeri "tanımsız" olarak yorumlanabilir
    /// </summary>
    public enum BorrowStatus
    {
        /// <summary>
        /// Kitap ödünç alınmış durumda
        /// Bu durum, kitap üyeye verildiğinde set edilir
        /// Varsayılan durumdur (default value)
        /// </summary>
        Borrowed = 1,

        /// <summary>
        /// Kitap iade edilmiş durumda
        /// Bu durum, kitap geri alındığında set edilir
        /// İşlem başarıyla tamamlanmış demektir
        /// </summary>
        Returned = 2,

        /// <summary>
        /// Kitap süresi geçmiş durumda
        /// Bu durum, due date geçtiğinde otomatik olarak set edilir
        /// Ceza hesaplaması bu durumda yapılır
        /// </summary>
        Overdue = 3,

        /// <summary>
        /// Kitap kaybolmuş durumda
        /// Bu durum, kitap bulunamadığında manuel olarak set edilir
        /// Özel işlem gerektirir (ceza, yeni kitap siparişi vb.)
        /// </summary>
        Lost = 4,

        /// <summary>
        /// Kitap hasarlı durumda
        /// Bu durum, kitap zarar gördüğünde manuel olarak set edilir
        /// Onarım veya değişim işlemi gerekebilir
        /// </summary>
        Damaged = 5,
    }
}
