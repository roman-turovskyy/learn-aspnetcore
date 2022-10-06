using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Example.DAL;

public class SampleDbContext : DbContext
{
    public virtual DbSet<Person> Person { get; set; } = null!;
    public virtual DbSet<BlogPost> BlogPost { get; set; } = null!;
    public virtual DbSet<Author> Author { get; set; } = null!;

    public SampleDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())");

            entity.Property(e => e.CreatedDate)
                .IsRequired()
                .HasColumnType("datetime")
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("(getutcdate())");

            entity.Property(e => e.ModifiedBy)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())");

            entity.Property(e => e.ModifiedDate)
                .IsRequired()
                .HasColumnType("datetime")
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("(getutcdate())");

            entity.Property(e => e.RowVersion)
                .IsRequired()
                .IsRowVersion();
        });

    }
}
