namespace LibraryApp.Domain.Common;

/// <summary>
/// Tüm domain entity'leri için temel sınıf
/// Bu sınıf, Clean Architecture'da Domain katmanının temelini oluşturur
/// Tüm entity'ler bu sınıftan türer ve ortak özellikleri paylaşır
/// 
/// Bu sınıfın amacı:
/// 1. Ortak property'leri tek yerde toplamak (DRY prensibi)
/// 2. Audit trail (iz sürme) özelliği sağlamak
/// 3. Soft delete (mantıksal silme) özelliği sağlamak
/// 4. Entity'lerin benzersizliğini garanti etmek (ID)
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Entity'nin benzersiz tanımlayıcısı
    /// Her entity'nin mutlaka bir ID'si olmalıdır
    /// Guid kullanılmasının sebepleri:
    /// - Global olarak benzersizdir
    /// - Tahmin edilmesi zordur (güvenlik)
    /// - Distributed sistemlerde çakışma riski yoktur
    /// - Veritabanında primary key olarak kullanılabilir
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Soft delete (mantıksal silme) flag'i
    /// Bu property true olduğunda entity silinmiş sayılır
    /// Ancak veritabanından fiziksel olarak silinmez
    /// 
    /// Soft delete kullanmanın avantajları:
    /// - Veri kaybı riski yoktur
    /// - Geri alma işlemi kolaydır
    /// - Audit trail korunur
    /// - İlişkili veriler korunur
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Entity'nin silinme tarihi
    /// Soft delete işlemi yapıldığında bu alan doldurulur
    /// Null ise entity silinmemiş demektir
    /// DateTimeOffset kullanılmasının sebebi:
    /// - Timezone bilgisini içerir
    /// - Uluslararası uygulamalarda tutarlılık sağlar
    /// </summary>
    public DateTimeOffset? DeletedOn { get; set; }

    /// <summary>
    /// Entity'nin oluşturulma tarihi
    /// Entity ilk kez veritabanına kaydedildiğinde doldurulur
    /// Bu alan, audit trail için önemlidir
    /// </summary>
    public DateTimeOffset? CreatedOn { get; set; }

    /// <summary>
    /// Entity'nin son güncellenme tarihi
    /// Entity her güncellendiğinde bu alan güncellenir
    /// Null ise entity hiç güncellenmemiş demektir
    /// </summary>
    public DateTimeOffset? UpdatedOn { get; set; }
}
