using Example.DAL;
using Example.Domain.Common;
using Example.Domain.Entities;
using Example.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

await CreateDatabases();
await SeedData();

static SampleDbContext GetDbContext()
{
    string? connStr = Environment.GetEnvironmentVariable("ExampleDbConnStr");
    if (connStr == null)
        throw new Exception("Environment variable ExampleDbConnStr is not defined.");

    var contextOptions = new DbContextOptionsBuilder<SampleDbContext>()
                        .UseSqlServer(connStr)
                        .Options;

    return new SampleDbContext(contextOptions);
}

static async Task CreateDatabases()
{

    using (var context = GetDbContext())
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}

static async Task SeedData()
{
    using (var context = GetDbContext())
    {
        context.Add(new Person
        {
            PersonId = "AB01E1E6-8402-4DC7-8950-1EC85B0C7023".G(),
            FirstName = "Roman",
            LastName = "Turovskyy",
            Sex = PersonSex.Male,
            Occupation = PersonOccupation.WorkingHard,
            OccupationReason = PersonOccupationReason.BecauseLikesToWork
        });
        context.Add(new Person
        {
            PersonId = "9135444A-7199-45C5-BFF4-671370176E61".G(),
            FirstName = "Master",
            LastName = "Yoda",
            Sex = null,
            Occupation = PersonOccupation.DoingNothing,
            OccupationReason = PersonOccupationReason.NobodyKnows
        });
        context.Add(new Person
        {
            PersonId = "BDAF4969-5406-4CDF-AD19-E34DF4837C34".G(),
            FirstName = "Sarah",
            LastName = "Connor",
            Sex = PersonSex.Female,
            Occupation = PersonOccupation.WorkingHard,
            OccupationReason = PersonOccupationReason.BecauseNeedsMoney
        });

        context.Add(new PersonLegacy { PersonLegacyId = "A53673EC-D6F2-4368-BA3F-C258ED63762D".G(), FirstName = "Roman", LastName = "Turovskyy Legacy" });
        context.Add(new PersonLegacy { PersonLegacyId = "5EC7C220-6FF6-4C6B-8987-AF8329B2DF2A".G(), FirstName = "Van", LastName = "Dam Legacy" });

        Product pacProduct = new Product { ShortName = "PAC" };
        context.Add(pacProduct);

        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(BlogStatus.Draft),
            Description = "Draft",
            SortOrder = 10,
            Entity = "BlogPost",
            Reference = "Status",
            Product = pacProduct
        });
        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(BlogStatus.OnReview),
            Description = "On Review",
            SortOrder = 20,
            Entity = "BlogPost",
            Reference = "Status",
            Product = pacProduct
        });
        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(BlogStatus.Published),
            Description = "Published",
            SortOrder = 30,
            Entity = "BlogPost",
            Reference = "Status",
            Product = pacProduct
        });


        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonSex.Unknown),
            Description = "Unknown",
            SortOrder = 10,
            Entity = "Person",
            Reference = "Sex",
            Product = pacProduct
        });
        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonSex.Male),
            Description = "Male",
            SortOrder = 20,
            Entity = "Person",
            Reference = "Sex",
            Product = pacProduct
        });
        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonSex.Female),
            Description = "Female",
            SortOrder = 10,
            Entity = "Person",
            Reference = "Sex",
            Product = pacProduct
        });


        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupation.WorkingHard),
            Description = "Working Hard",
            SortOrder = 10,
            Entity = "Person",
            Reference = "Occupation",
            Product = pacProduct
        });
        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupation.DoingNothing),
            Description = "Doing Nothing",
            SortOrder = 20,
            Entity = "Person",
            Reference = "Occupation",
            Product = pacProduct
        });


        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupationReason.NobodyKnows),
            Description = "Nobody Knows",
            SortOrder = 10,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });
        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupationReason.BecauseIsLazy),
            Description = "Because Is Lazy",
            SortOrder = 20,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });
        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupationReason.BecauseNeedsMoney),
            Description = "Because Needs Money",
            SortOrder = 30,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });
        context.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupationReason.BecauseLikesToWork),
            Description = "Because Likes To Work",
            SortOrder = 40,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });


        await context.SaveChangesAsync();
    }
}

static Guid GetEnumKey<T>(T value) where T : Enum
{
    return typeof(T)?.GetField(value.ToString())?.GetCustomAttribute<ReferenceIdAttribute>()?.ReferenceId
        ?? throw new InvalidCastException($"ReferenceIdAttribute must be present for {value}.");
}

static class StringExtensions
{
    public static Guid G(this string sGuid)
    {
        return new Guid(sGuid);
    }
}