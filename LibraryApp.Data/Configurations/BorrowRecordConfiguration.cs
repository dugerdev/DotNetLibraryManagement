using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.Data.Configurations;

// BorrowRecord entity'si için Entity Framework konfigürasyonu
public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
{
    public void Configure(EntityTypeBuilder<BorrowRecord> builder)
    {
        // Tablo adını belirle
        builder.ToTable("BorrowRecords");

        // Primary key tanımla
        builder.HasKey(br => br.Id);

        // Property konfigürasyonları
        builder.Property(br => br.BorrowDate)
            .IsRequired();                   // Zorunlu alan

        builder.Property(br => br.DueDate)
            .IsRequired();                   // Zorunlu alan

        builder.Property(br => br.ReturnDate)
            .IsRequired(false);              // Opsiyonel alan

        builder.Property(br => br.Status)
            .IsRequired()                    // Zorunlu alan
            .HasDefaultValue(BorrowStatus.Borrowed); // Default değer Borrowed

        builder.Property(br => br.FineAmount)
            .IsRequired(false)               // Opsiyonel alan
            .HasColumnType("decimal(18,2)");     // Para formatı

        builder.Property(br => br.Notes)
            .IsRequired(false)               // Opsiyonel alan
            .HasMaxLength(1000);            // Maksimum 1000 karakter

        // Foreign key'ler
        builder.Property(br => br.BookId)
            .IsRequired();                   // Zorunlu alan

        builder.Property(br => br.MemberId)
            .IsRequired();                   // Zorunlu alan

        // Audit field'ları
        builder.Property(br => br.CreatedOn)
            .IsRequired();                   // Zorunlu alan

        builder.Property(br => br.UpdatedOn)
            .IsRequired(false);              // Opsiyonel alan

        builder.Property(br => br.DeletedOn)
            .IsRequired(false);              // Opsiyonel alan

        builder.Property(br => br.IsDeleted)
            .IsRequired()                    // Zorunlu alan
            .HasDefaultValue(false);         // Default değer false

        // Navigation property konfigürasyonları
        builder.HasOne(br => br.Book)        // Bir ödünç kaydı bir kitap
            .WithMany(b => b.BorrowRecords)  // Bir kitap birden fazla ödünç kaydı
            .HasForeignKey(br => br.BookId)  // Foreign key
            .OnDelete(DeleteBehavior.Restrict); // Silme davranışı: Restrict

        builder.HasOne(br => br.Member)      // Bir ödünç kaydı bir üye
            .WithMany(m => m.BorrowRecords)  // Bir üye birden fazla ödünç kaydı
            .HasForeignKey(br => br.MemberId) // Foreign key
            .OnDelete(DeleteBehavior.Restrict); // Silme davranışı: Restrict

        // Index'ler - Performans için
        builder.HasIndex(br => br.BookId);    // Kitap ID index'i
        builder.HasIndex(br => br.MemberId);  // Üye ID index'i
        builder.HasIndex(br => br.BorrowDate); // Ödünç verme tarihi index'i
        builder.HasIndex(br => br.DueDate);    // Geri verme tarihi index'i
        builder.HasIndex(br => br.Status);     // Durum index'i
        builder.HasIndex(br => br.IsDeleted);  // Soft delete index'i
    }
}