using Example.Common.Database.Enums;
using Example.Domain.Entities;
using Example.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

namespace Example.DAL;

public class SampleDbContext : DbContext
{
    public virtual DbSet<Person> Person { get; set; }
    public virtual DbSet<PersonLegacy> PersonLegacy { get; set; }
    public virtual DbSet<BlogPost> BlogPost { get; set; }
    public virtual DbSet<Author> Author { get; set; }

    public virtual DbSet<Product> Product { get; set; }
    public virtual DbSet<ReferenceByProduct> ReferenceByProduct { get; set; }
    //public virtual DbSet<ReferenceChild> ReferenceChild { get; set; }

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

            entity.Property(e => e.StatusStr)
                .HasConversion<string>();

            entity.Property(e => e.StatusInt);

            entity.Property(e => e.Sex)
                .HasConversion(new ReferenceEnumConverter<PersonSex>());

            entity.Property(e => e.Occupation)
                .IsRequired()
                .HasConversion(new ReferenceEnumConverter<PersonOccupation>());

            entity.Property(e => e.OccupationReason)
                .IsRequired()
                .HasConversion(new ReferenceEnumConverter<PersonOccupationReason>());

            // TODO: deal with nullability
            entity.Property(e => e.Sex2)
                .HasConversion(new ReferenceEnumConverter2<PersonSex2>());

            entity.Property(e => e.Occupation2)
                .IsRequired()
                .HasConversion(new ReferenceEnumConverter2<PersonOccupation2>());

            entity.Property(e => e.OccupationReason2)
                .IsRequired()
                .HasConversion(new ReferenceEnumConverter2<PersonOccupationReason2>());

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

        modelBuilder.Entity<ReferenceByProduct>(entity =>
        {
            entity.HasKey(x => x.ReferenceByProductId);

            entity.HasOne(x => x.Product)
                 .WithMany()
                 .HasForeignKey(x => x.ProductId);
        });

        //modelBuilder.Entity<ReferenceChild>(entity =>
        //{
        //    entity.HasKey("ReferenceId", "ChildId");

        //    modelBuilder.Entity<ReferenceChild>()
        //         .HasOne(e => e.Reference)
        //         .WithMany(x => x.Children)
        //         .HasForeignKey(x => x.ReferenceId);

        //    modelBuilder.Entity<ReferenceChild>()
        //         .HasOne(e => e.Child)
        //         .WithMany()
        //         .HasForeignKey(x => x.ChildId);
        //});
    }
}
