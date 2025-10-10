using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(c => c.CreatedOn)
             .IsRequired();                  

        builder.Property(c => c.UpdatedOn)
            .IsRequired(false);              

        builder.Property(c => c.DeletedOn)
            .IsRequired(false);              

        builder.Property(c => c.IsDeleted)
            .IsRequired()                    
            .HasDefaultValue(false);
        
        builder.HasMany(c => c.Books)
            .WithOne(b => b.Category)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Name)       
           .IsUnique();                    
        builder.HasIndex(c => c.IsDeleted);  


    }
}
