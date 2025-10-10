using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.Data.Configurations;

// Member entity'si için Entity Framework konfigürasyonu
public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        // Tablo adını belirle
        builder.ToTable("Members");

        // Primary key tanımla
        builder.HasKey(m => m.Id);

        // Property konfigürasyonları
        builder.Property(m => m.FirstName)
            .IsRequired()                    // Zorunlu alan
            .HasMaxLength(100);             // Maksimum 100 karakter

        builder.Property(m => m.LastName)
            .IsRequired()                    // Zorunlu alan
            .HasMaxLength(100);             // Maksimum 100 karakter

        builder.Property(m => m.Email)
            .IsRequired()                    // Zorunlu alan
            .HasMaxLength(255);             // Maksimum 255 karakter

        builder.Property(m => m.PhoneNumber)
            .IsRequired()                    // Zorunlu alan
            .HasMaxLength(20);              // Maksimum 20 karakter

        builder.Property(m => m.Address)
            .IsRequired()                    // Zorunlu alan
            .HasMaxLength(500);             // Maksimum 500 karakter

        builder.Property(m => m.MembershipDate)
            .IsRequired();                   // Zorunlu alan

        builder.Property(m => m.ExpirationDate)
            .IsRequired(false);              // Opsiyonel alan

        builder.Property(m => m.IsActive)
            .IsRequired()                    // Zorunlu alan
            .HasDefaultValue(true);          // Default değer true

        // Audit field'ları
        builder.Property(m => m.CreatedOn)
            .IsRequired();                   // Zorunlu alan

        builder.Property(m => m.UpdatedOn)
            .IsRequired(false);              // Opsiyonel alan

        builder.Property(m => m.DeletedOn)
            .IsRequired(false);              // Opsiyonel alan

        builder.Property(m => m.IsDeleted)
            .IsRequired()                    // Zorunlu alan
            .HasDefaultValue(false);         // Default değer false

        // Navigation property konfigürasyonu
        builder.HasMany(m => m.BorrowRecords) // Bir üye birden fazla ödünç kaydı
            .WithOne(br => br.Member)         // Bir ödünç kaydı bir üye
            .HasForeignKey(br => br.MemberId) // Foreign key
            .OnDelete(DeleteBehavior.Restrict); // Silme davranışı: Restrict

        // Index'ler - Performans için
        builder.HasIndex(m => m.FirstName);   // İsim index'i
        builder.HasIndex(m => m.LastName);    // Soyisim index'i
        builder.HasIndex(m => m.Email)        // E-posta index'i (unique olmalı)
            .IsUnique();                      // Benzersiz olmalı
        builder.HasIndex(m => m.IsActive);    // Aktif durum index'i
        builder.HasIndex(m => m.IsDeleted);   // Soft delete index'i
    }
}