using LibraryApp.Domain.Common;
using LibraryApp.Domain.Enums;

namespace LibraryApp.Domain.Entities;

/// <summary>
/// Ödünç verme kaydı (BorrowRecord) domain entity'si
/// Bu sınıf, kütüphane sistemindeki kitap ödünç verme kayıtlarını temsil eder
/// 
/// Bu entity'nin özellikleri:
/// 1. Ödünç verme sürecini yönetir (ödünç alma, iade, gecikme)
/// 2. Business logic içerir (gecikme hesaplaması, süre hesaplaması)
/// 3. Kitap ve üye ile ilişki kurar
/// 4. Ödünç verme durumunu takip eder
/// </summary>
public class BorrowRecord : EntityBase
{
    // ========== BASIC PROPERTIES (Temel Özellikler) ==========
    
    /// <summary>
    /// Ödünç alma tarihi
    /// Kitabın üyeye verildiği tarih
    /// </summary>
    public DateTime BorrowDate { get; set; }

    /// <summary>
    /// Geri verme tarihi (due date)
    /// Kitabın geri verilmesi gereken tarih
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Geri verme tarihi
    /// Kitabın gerçekte geri verildiği tarih
    /// Null ise henüz geri verilmemiş demektir
    /// </summary>
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// Ödünç verme durumu
    /// BorrowStatus enum'undan bir değer
    /// Varsayılan değer: Borrowed
    /// </summary>
    public BorrowStatus Status { get; set; } = BorrowStatus.Borrowed;

    /// <summary>
    /// Ceza miktarı
    /// Gecikme durumunda uygulanan ceza
    /// Null ise ceza yok demektir
    /// </summary>
    public decimal? FineAmount { get; set; }

    /// <summary>
    /// Notlar
    /// Ödünç verme kaydı hakkında ek bilgiler
    /// </summary>
    public string? Notes { get; set; }

    // ========== FOREIGN KEYS (Dış Anahtarlar) ==========
    
    /// <summary>
    /// Ödünç verilen kitabın ID'si
    /// Book entity'sine referans
    /// </summary>
    public Guid BookId { get; set; }

    /// <summary>
    /// Ödünç alan üyenin ID'si
    /// Member entity'sine referans
    /// </summary>
    public Guid MemberId { get; set; }

    // ========== NAVIGATION PROPERTIES (İlişki Property'leri) ==========
    
    /// <summary>
    /// Ödünç verilen kitap
    /// Many-to-One ilişki: Bir ödünç kaydı bir kitaba ait
    /// </summary>
    public Book Book { get; set; } = null!;

    /// <summary>
    /// Ödünç alan üye
    /// Many-to-One ilişki: Bir ödünç kaydı bir üyeye ait
    /// </summary>
    public Member Member { get; set; } = null!;

    // ========== COMPUTED PROPERTIES (Hesaplanan Property'ler) ==========
    
    /// <summary>
    /// Kitap gecikmiş mi?
    /// Due date geçmiş ve henüz geri verilmemişse true
    /// </summary>
    public bool IsOverDue => DateTime.Now > DueDate && Status == BorrowStatus.Borrowed;

    /// <summary>
    /// Gecikme gün sayısı
    /// Kaç gün gecikmiş olduğunu hesaplar
    /// </summary>
    public int DaysOverdue => IsOverDue ? (DateTime.Now - DueDate).Days : 0;

    /// <summary>
    /// Ödünç alma süresi (gün cinsinden)
    /// Ödünç alma tarihinden itibaren geçen gün sayısı
    /// </summary>
    public int BorrowDuration => ReturnDate.HasValue ?
       (ReturnDate.Value - BorrowDate).Days :
       (DateTime.Now - BorrowDate).Days;
}
