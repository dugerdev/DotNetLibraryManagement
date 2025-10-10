using LibraryApp.Domain.Common;

namespace LibraryApp.Domain.Entities;

/// <summary>
/// Yazar (Author) domain entity'si
/// Bu sınıf, kütüphane sistemindeki yazarları temsil eder
/// EntityBase'den türer, bu sayede ID, audit fields ve soft delete özelliklerini alır
/// 
/// Domain Entity'nin özellikleri:
/// 1. Business logic içerir (FullName, Age gibi computed property'ler)
/// 2. Navigation property'ler ile diğer entity'lerle ilişki kurar
/// 3. Immutable olmayabilir (setter'lar var)
/// 4. Veritabanı detaylarından bağımsızdır
/// </summary>
public class Author : EntityBase
{
    /// <summary>
    /// Yazarın adı
    /// Zorunlu alan, boş olamaz
    /// Veritabanında nvarchar(100) olarak tanımlanır
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Yazarın soyadı
    /// Zorunlu alan, boş olamaz
    /// Veritabanında nvarchar(100) olarak tanımlanır
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Yazarın biyografisi
    /// Opsiyonel alan, null olabilir
    /// Veritabanında nvarchar(1000) olarak tanımlanır
    /// </summary>
    public string? Biography { get; set; }

    /// <summary>
    /// Yazarın doğum tarihi
    /// Opsiyonel alan, null olabilir
    /// Veritabanında datetime2 olarak tanımlanır
    /// </summary>
    public DateTime? BirthDate { get; set; }

    // ========== NAVIGATION PROPERTIES (İlişki Property'leri) ==========
    
    /// <summary>
    /// Yazarın yazdığı kitaplar
    /// One-to-Many ilişki: Bir yazar birden fazla kitap yazabilir
    /// Collection initialization syntax (C# 12) kullanılmış
    /// Bu sayede null reference exception riski ortadan kalkar
    /// </summary>
    public ICollection<Book> Books { get; set; } = [];

    // ========== COMPUTED PROPERTIES (Hesaplanan Property'ler) ==========
    
    /// <summary>
    /// Yazarın tam adı (Ad + Soyad)
    /// Bu property, FirstName ve LastName'i birleştirir
    /// Computed property olduğu için veritabanında kolon yoktur
    /// Business logic içerir: ad ve soyadı birleştirme kuralı
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Yazarın yaşı
    /// Doğum tarihinden hesaplanır
    /// Doğum tarihi yoksa null döner
    /// 
    /// Not: Bu hesaplama basit bir yöntemdir (sadece yıl farkı)
    /// Gerçek uygulamalarda daha detaylı yaş hesaplaması yapılabilir
    /// </summary>
    public int? Age => BirthDate.HasValue ?
       DateTime.Now.Year - BirthDate.Value.Year : null;
}
