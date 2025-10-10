using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data.Context;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    protected LibraryDbContext()
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }
    public DbSet<Member> Members { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.AuthorConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.BookConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.MemberConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.BorrowRecordConfiguration());

        base.OnModelCreating(modelBuilder);

    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is LibraryApp.Domain.Common.EntityBase &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (LibraryApp.Domain.Common.EntityBase)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedOn = DateTimeOffset.Now;
                entity.Id = Guid.NewGuid();
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdatedOn = DateTimeOffset.Now;
            }
        }

    }
}
