using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.Data.Configurations;

// Book entity'si için Entity Framework konfigürasyonu
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        // Tablo adını belirle
        builder.ToTable("Books");

        // Primary key tanımla
        builder.HasKey(b => b.Id);

        // Property konfigürasyonları
        builder.Property(b => b.Title)
            .IsRequired()                    // Zorunlu alan
            .HasMaxLength(200);             // Maksimum 200 karakter

        builder.Property(b => b.ISBN)
            .IsRequired()                    // Zorunlu alan
            .HasMaxLength(20);              // Maksimum 20 karakter

        builder.Property(b => b.Description)
            .IsRequired(false)               // Opsiyonel alan
            .HasColumnType("nvarchar(max)");    // Sınırsız metin

        builder.Property(b => b.PageCount)
            .IsRequired();                   // Zorunlu alan

        builder.Property(b => b.PublishedDate)
            .IsRequired();                   // Zorunlu alan

        builder.Property(b => b.AvailableCopies)
            .IsRequired()                    // Zorunlu alan
            .HasDefaultValue(0);             // Default değer 0

        builder.Property(b => b.TotalCopies)
            .IsRequired()                    // Zorunlu alan
            .HasDefaultValue(0);             // Default değer 0

        // Foreign key'ler
        builder.Property(b => b.AuthorId)
            .IsRequired();                   // Zorunlu alan

        builder.Property(b => b.CategoryId)
            .IsRequired();                   // Zorunlu alan

        // Audit field'ları
        builder.Property(b => b.CreatedOn)
            .IsRequired();                   // Zorunlu alan

        builder.Property(b => b.UpdatedOn)
            .IsRequired(false);              // Opsiyonel alan

        builder.Property(b => b.DeletedOn)
            .IsRequired(false);              // Opsiyonel alan

        builder.Property(b => b.IsDeleted)
            .IsRequired()                    // Zorunlu alan
            .HasDefaultValue(false);         // Default değer false

        // Navigation property konfigürasyonları
        builder.HasOne(b => b.Author)        // Bir kitap bir yazar
            .WithMany(a => a.Books)          // Bir yazar birden fazla kitap
            .HasForeignKey(b => b.AuthorId)  // Foreign key
            .OnDelete(DeleteBehavior.Restrict); // Silme davranışı: Restrict

        builder.HasOne(b => b.Category)      // Bir kitap bir kategori
            .WithMany(c => c.Books)          // Bir kategori birden fazla kitap
            .HasForeignKey(b => b.CategoryId) // Foreign key
            .OnDelete(DeleteBehavior.Restrict); // Silme davranışı: Restrict

        builder.HasMany(b => b.BorrowRecords) // Bir kitap birden fazla ödünç kaydı
            .WithOne(br => br.Book)           // Bir ödünç kaydı bir kitap
            .HasForeignKey(br => br.BookId)   // Foreign key
            .OnDelete(DeleteBehavior.Restrict); // Silme davranışı: Restrict

        // Index'ler - Performans için
        builder.HasIndex(b => b.Title);       // Başlık index'i
        builder.HasIndex(b => b.ISBN)         // ISBN index'i (unique olmalı)
            .IsUnique();                      // Benzersiz olmalı
        builder.HasIndex(b => b.AuthorId);    // Yazar ID index'i
        builder.HasIndex(b => b.CategoryId);  // Kategori ID index'i
        builder.HasIndex(b => b.IsDeleted);   // Soft delete index'i
    }
}