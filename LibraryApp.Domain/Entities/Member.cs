using LibraryApp.Domain.Common;
using LibraryApp.Domain.Enums;

namespace LibraryApp.Domain.Entities;

/// <summary>
/// Üye (Member) domain entity'si
/// Bu sınıf, kütüphane sistemindeki üyeleri temsil eder
/// 
/// Bu entity'nin özellikleri:
/// 1. Üyelik bilgilerini yönetir
/// 2. Üyelik geçerliliğini kontrol eder
/// 3. Ödünç alma limitlerini takip eder
/// 4. Ödünç verme kayıtları ile ilişki kurar
/// </summary>
public class Member : EntityBase
{
    // ========== BASIC PROPERTIES (Temel Özellikler) ==========
    
    /// <summary>
    /// Üyenin adı
    /// Zorunlu alan, boş olamaz
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Üyenin soyadı
    /// Zorunlu alan, boş olamaz
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Üyenin email adresi
    /// Zorunlu alan, benzersiz olmalı
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Üyenin telefon numarası
    /// Zorunlu alan, benzersiz olmalı
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Üyenin adresi
    /// Zorunlu alan
    /// </summary>
    public string Address { get; set; } = string.Empty;

    // ========== MEMBERSHIP PROPERTIES (Üyelik Özellikleri) ==========
    
    /// <summary>
    /// Üyelik başlangıç tarihi
    /// Üyenin sisteme kaydolduğu tarih
    /// </summary>
    public DateTime? MembershipDate { get; set; }

    /// <summary>
    /// Üyelik bitiş tarihi
    /// Üyeliğin geçerliliğini sona erdiği tarih
    /// Null ise süresiz üyelik demektir
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Üye aktif mi?
    /// False ise üye sisteme giriş yapamaz
    /// </summary>
    public bool IsActive { get; set; } = true;

    // ========== NAVIGATION PROPERTIES (İlişki Property'leri) ==========
    
    /// <summary>
    /// Üyenin ödünç verme kayıtları
    /// One-to-Many ilişki: Bir üye birden fazla ödünç verme kaydı olabilir
    /// </summary>
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = [];

    // ========== COMPUTED PROPERTIES (Hesaplanan Property'ler) ==========
    
    /// <summary>
    /// Üyenin tam adı (Ad + Soyad)
    /// Computed property: FirstName ve LastName'i birleştirir
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Üyelik geçerli mi?
    /// Üye aktif olmalı ve üyelik süresi dolmamış olmalı
    /// Business rule: Üyelik geçersizse kitap ödünç alamaz
    /// </summary>
    public bool IsMemberShipValid => IsActive &&
        (ExpirationDate == null || ExpirationDate > DateTime.Now);

    /// <summary>
    /// Aktif ödünç alma sayısı
    /// Henüz geri verilmemiş kitap sayısı
    /// Business rule: Maksimum limit kontrolü için kullanılır
    /// </summary>
    public int ActiveBorrowCount => BorrowRecords.Count(r => r.Status == BorrowStatus.Borrowed);
}
