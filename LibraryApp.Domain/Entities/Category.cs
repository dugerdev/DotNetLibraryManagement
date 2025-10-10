using LibraryApp.Domain.Common;

namespace LibraryApp.Domain.Entities;

/// <summary>
/// Kategori (Category) domain entity'si
/// Bu sınıf, kütüphane sistemindeki kitap kategorilerini temsil eder
/// 
/// Bu entity'nin özellikleri:
/// 1. Kitap kategorilerini organize eder
/// 2. Kitaplarla One-to-Many ilişki kurar
/// 3. Kategori bazında kitap sayısını takip eder
/// 4. Kategorilere göre arama ve filtreleme sağlar
/// </summary>
public class Category : EntityBase
{
    // ========== BASIC PROPERTIES (Temel Özellikler) ==========
    
    /// <summary>
    /// Kategori adı
    /// Zorunlu alan, benzersiz olmalı
    /// Örnek: "Roman", "Bilim", "Tarih", "Edebiyat"
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategori açıklaması
    /// Opsiyonel alan, kategori hakkında detaylı bilgi
    /// </summary>
    public string? Description { get; set; }

    // ========== NAVIGATION PROPERTIES (İlişki Property'leri) ==========
    
    /// <summary>
    /// Kategoriye ait kitaplar
    /// One-to-Many ilişki: Bir kategori birden fazla kitaba sahip olabilir
    /// Collection initialization syntax (C# 12) kullanılmış
    /// </summary>
    public ICollection<Book> Books { get; set; } = [];

    // ========== COMPUTED PROPERTIES (Hesaplanan Property'ler) ==========
    
    /// <summary>
    /// Kategoriye ait kitap sayısı
    /// Computed property: Books collection'ının Count'u
    /// Bu property, kategori bazında istatistik için kullanılır
    /// </summary>
    public int BookCount => Books.Count;
}
