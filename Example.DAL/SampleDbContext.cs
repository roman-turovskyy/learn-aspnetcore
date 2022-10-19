using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Example.DAL;

public class SampleDbContext : DbContext
{
    public virtual DbSet<Person> Person { get; set; }
    public virtual DbSet<PersonLegacy> PersonLegacy { get; set; }
    public virtual DbSet<BlogPost> BlogPost { get; set; }
    public virtual DbSet<Author> Author { get; set; }

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

        modelBuilder.Entity<PersonLegacy>(entity =>
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

            entity.Property(e => e.CreatedOn)
                .IsRequired()
                .HasColumnType("datetime")
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("(getutcdate())");

            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ModifiedOn)
                .HasColumnType("datetime")
                .HasConversion(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            entity.Property(e => e.RowVersion)
                .IsRequired()
                .IsRowVersion();
        });
    }
}
