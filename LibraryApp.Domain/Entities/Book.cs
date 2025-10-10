using LibraryApp.Domain.Common;

namespace LibraryApp.Domain.Entities;

/// <summary>
/// Kitap (Book) domain entity'si
/// Bu sınıf, kütüphane sistemindeki kitapları temsil eder
/// En karmaşık entity'lerden biridir çünkü birden fazla ilişki içerir
/// 
/// Bu entity'nin özellikleri:
/// 1. Çok sayıda business rule içerir (müsaitlik, ödünç alma kuralları)
/// 2. Birden fazla navigation property içerir
/// 3. Computed property'ler ile business logic sağlar
/// 4. Foreign key'ler ile diğer entity'lere referans verir
/// </summary>
public class Book : EntityBase
{
    // ========== BASIC PROPERTIES (Temel Özellikler) ==========
    
    /// <summary>
    /// Kitabın başlığı
    /// Zorunlu alan, boş olamaz
    /// Veritabanında nvarchar(200) olarak tanımlanır
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Kitabın ISBN numarası
    /// Zorunlu alan, benzersiz olmalı
    /// Veritabanında nvarchar(20) olarak tanımlanır
    /// ISBN-10 veya ISBN-13 formatında olabilir
    /// </summary>
    public string ISBN { get; set; } = string.Empty;

    /// <summary>
    /// Kitabın açıklaması
    /// Opsiyonel alan, null olabilir
    /// Veritabanında nvarchar(max) olarak tanımlanır
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Kitabın sayfa sayısı
    /// Zorunlu alan, pozitif olmalı
    /// Veritabanında int olarak tanımlanır
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// Kitabın yayın tarihi
    /// Zorunlu alan
    /// Veritabanında datetime2 olarak tanımlanır
    /// </summary>
    public DateTime PublishedDate { get; set; }

    // ========== INVENTORY PROPERTIES (Envanter Özellikleri) ==========
    
    /// <summary>
    /// Müsait (ödünç alınabilir) kopya sayısı
    /// Bu sayı, ödünç verme/iade işlemlerinde değişir
    /// Hiçbir zaman negatif olamaz
    /// Hiçbir zaman TotalCopies'den fazla olamaz
    /// </summary>
    public int AvailableCopies { get; set; }

    /// <summary>
    /// Toplam kopya sayısı
    /// Kütüphanedeki toplam kitap sayısı
    /// Hiçbir zaman negatif olamaz
    /// AvailableCopies'den küçük olamaz
    /// </summary>
    public int TotalCopies { get; set; }

    // ========== FOREIGN KEYS (Dış Anahtarlar) ==========
    
    /// <summary>
    /// Kitabın yazarının ID'si
    /// Author entity'sine referans
    /// Zorunlu alan, null olamaz
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Kitabın kategorisinin ID'si
    /// Category entity'sine referans
    /// Zorunlu alan, null olamaz
    /// </summary>
    public Guid CategoryId { get; set; }

    // ========== NAVIGATION PROPERTIES (İlişki Property'leri) ==========
    
    /// <summary>
    /// Kitabın ödünç verme kayıtları
    /// One-to-Many ilişki: Bir kitap birden fazla ödünç verme kaydı olabilir
    /// Collection initialization syntax kullanılmış
    /// </summary>
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = [];

    /// <summary>
    /// Kitabın yazarı
    /// Many-to-One ilişki: Bir kitap bir yazara ait
    /// null! operatörü ile null-forgiving yapılmış
    /// Bu, EF Core'un lazy loading için gerekli
    /// </summary>
    public Author Author { get; set; } = null!;

    /// <summary>
    /// Kitabın kategorisi
    /// Many-to-One ilişki: Bir kitap bir kategoriye ait
    /// null! operatörü ile null-forgiving yapılmış
    /// </summary>
    public Category Category { get; set; } = null!;

    // ========== COMPUTED PROPERTIES (Hesaplanan Property'ler) ==========
    
    /// <summary>
    /// Kitap müsait mi?
    /// AvailableCopies > 0 ise true
    /// Bu property, basit müsaitlik kontrolü yapar
    /// </summary>
    public bool IsAvailable => AvailableCopies > 0;

    /// <summary>
    /// Kitap ödünç alınabilir mi?
    /// Hem müsait olmalı hem de silinmemiş olmalı
    /// Bu property, ödünç alma öncesi kontrol için kullanılır
    /// Business rule: Silinmiş kitaplar ödünç alınamaz
    /// </summary>
    public bool CanBeBorrowed => IsAvailable && !IsDeleted;

    /// <summary>
    /// Ödünç alınan kopya sayısı
    /// TotalCopies - AvailableCopies formülü ile hesaplanır
    /// Bu property, kaç kopyanın ödünç alındığını gösterir
    /// </summary>
    public int BorrowedCopies => TotalCopies - AvailableCopies;
}
