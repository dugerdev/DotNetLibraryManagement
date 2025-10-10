using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.Data.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Biography)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(a => a.BirthDate)
            .IsRequired(false);

        // AuiditFiledlari 
        builder.Property(a => a.CreatedOn)
            .IsRequired();

        builder.Property(a => a.UpdatedOn)
            .IsRequired(false);

        builder.Property(a => a.DeletedOn)
            .IsRequired(false);

        builder.Property(a => a.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasMany(a => a.Books)
            .WithOne(a => a.Author)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.FirstName);   
        builder.HasIndex(a => a.LastName);    
        builder.HasIndex(a => a.IsDeleted);  

    }
}
