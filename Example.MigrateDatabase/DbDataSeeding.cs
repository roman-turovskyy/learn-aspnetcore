using Example.Common.Database.Enums;
using Example.DAL;
using Example.Domain.Common;
using Example.Domain.Entities;
using Example.Domain.Enums;
using System.Reflection;

namespace Example.MigrateDatabase;

internal class DbDataSeeding
{
    private SampleDbContext _context;

    public DbDataSeeding(SampleDbContext context)
    {
        _context = context;
    }

    internal async Task SeedData()
    {
        SeedEnums();

        ReferenceEnumConverterCache.SetReferenceByProductProvider(new ReferenceByProductProvider(_context));

        _context.Add(new Person
        {
            PersonId = "AB01E1E6-8402-4DC7-8950-1EC85B0C7023".G(),
            FirstName = "Roman",
            LastName = "Turovskyy",
            Sex = PersonSex.Male,
            Occupation = PersonOccupation.WorkingHard,
            OccupationReason = PersonOccupationReason.BecauseLikesToWork,
            Sex2 = PersonSex2.Male,
            Occupation2 = PersonOccupation2.WorkingHard,
            OccupationReason2 = PersonOccupationReason2.BecauseLikesToWork
        });
        _context.Add(new Person
        {
            PersonId = "9135444A-7199-45C5-BFF4-671370176E61".G(),
            FirstName = "Master",
            LastName = "Yoda",
            Sex = null,
            Occupation = PersonOccupation.DoingNothing,
            OccupationReason = PersonOccupationReason.NobodyKnows,
            Sex2 = null,
            Occupation2 = PersonOccupation2.DoingNothing,
            OccupationReason2 = PersonOccupationReason2.NobodyKnows
        });
        _context.Add(new Person
        {
            PersonId = "BDAF4969-5406-4CDF-AD19-E34DF4837C34".G(),
            FirstName = "Sarah",
            LastName = "Connor",
            Sex = PersonSex.Female,
            Occupation = PersonOccupation.WorkingHard,
            OccupationReason = PersonOccupationReason.BecauseNeedsMoney,
            Sex2 = PersonSex2.Female,
            Occupation2 = PersonOccupation2.WorkingHard,
            OccupationReason2 = PersonOccupationReason2.BecauseNeedsMoney
        });

        _context.Add(new PersonLegacy { PersonLegacyId = "A53673EC-D6F2-4368-BA3F-C258ED63762D".G(), FirstName = "Roman", LastName = "Turovskyy Legacy" });
        _context.Add(new PersonLegacy { PersonLegacyId = "5EC7C220-6FF6-4C6B-8987-AF8329B2DF2A".G(), FirstName = "Van", LastName = "Dam Legacy" });

        await _context.SaveChangesAsync();
    }

    private void SeedEnums()
    {
        Product pacProduct = new Product { ShortName = "DME" };
        _context.Add(pacProduct);

        var enums = new List<ReferenceByProduct>();

        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(BlogStatus.Draft),
            Description = "Draft",
            SortOrder = 10,
            Entity = "BlogPost",
            Reference = "Status",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(BlogStatus.OnReview),
            Description = "On Review",
            SortOrder = 20,
            Entity = "BlogPost",
            Reference = "Status",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(BlogStatus.Published),
            Description = "Published",
            SortOrder = 30,
            Entity = "BlogPost",
            Reference = "Status",
            Product = pacProduct
        });


        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonSex.Unknown),
            Description = "Unknown",
            SortOrder = 10,
            Entity = "Person",
            Reference = "Sex",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonSex.Male),
            Description = "Male",
            SortOrder = 20,
            Entity = "Person",
            Reference = "Sex",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonSex.Female),
            Description = "Female",
            SortOrder = 10,
            Entity = "Person",
            Reference = "Sex",
            Product = pacProduct
        });


        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupation.WorkingHard),
            Description = "Working Hard",
            SortOrder = 10,
            Entity = "Person",
            Reference = "Occupation",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupation.DoingNothing),
            Description = "Doing Nothing",
            SortOrder = 20,
            Entity = "Person",
            Reference = "Occupation",
            Product = pacProduct
        });


        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupationReason.NobodyKnows),
            Description = "Nobody Knows",
            SortOrder = 10,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupationReason.BecauseIsLazy),
            Description = "Because Is Lazy",
            SortOrder = 20,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupationReason.BecauseNeedsMoney),
            Description = "Because Needs Money",
            SortOrder = 30,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = GetEnumKey(PersonOccupationReason.BecauseLikesToWork),
            Description = "Because Likes To Work",
            SortOrder = 40,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });

        foreach (var e in enums)
            e.Code = e.Description.Replace(" ", "");

        _context.AddRange(enums);

        // Force save changes after seeding enums so data that depends on enums is available for the ReferenceByProductProvider
        _context.SaveChanges();
    }

    private static Guid GetEnumKey<T>(T value) where T : Enum
    {
        return typeof(T)?.GetField(value.ToString())?.GetCustomAttribute<ReferenceIdAttribute>()?.ReferenceId
            ?? throw new InvalidCastException($"ReferenceIdAttribute must be present for {value}.");
    }
}
