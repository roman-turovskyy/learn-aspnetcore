using Example.Common.Database.Enums;
using Example.DAL;
using Example.Domain.Entities;
using Example.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Example.MigrateDatabase;

internal class DbDataSeeding
{
    private SampleDbContext _context;
    private Guid _playingGamesOccupatiodId = "00000CE7-7F8A-45C3-8F72-EB8F016C63CF".G();

    public DbDataSeeding(SampleDbContext context)
    {
        _context = context;
    }

    internal async Task SeedData()
    {
        SeedEnums();

        var memCache = new MemoryCache(new MemoryCacheOptions());

        ReferenceByProductProviderStaticHost.SetReferenceByProductProvider(new ReferenceByProductProvider(_context, memCache));

        _context.Add(new Person
        {
            PersonId = "AB01E1E6-8402-4DC7-8950-1EC85B0C7023".G(),
            FirstName = "Roman",
            LastName = "Turovskyy",
            StatusStr = PersonStatus.Busy,
            StatusInt = PersonStatus.Busy,
            Sex = PersonSex.Male,
            Occupation = PersonOccupation.WorkingHard,
            OccupationReason = PersonOccupationReason.BecauseLikesToWork,
            Sex2 = PersonSex2.Male,
            Occupation2 = PersonOccupation2.WorkingHard,
            OccupationReason2 = PersonOccupationReason2.BecauseLikesToWork,
            Occupation22 = PersonOccupation2.WorkingHard
        });
        _context.Add(new Person
        {
            PersonId = "9135444A-7199-45C5-BFF4-671370176E61".G(),
            FirstName = "Master",
            LastName = "Yoda",
            StatusStr = null,
            StatusInt = null,
            Sex = null,
            Occupation = PersonOccupation.DoingNothing,
            OccupationReason = PersonOccupationReason.NobodyKnows,
            Sex2 = null,
            Occupation2 = PersonOccupation2.DoingNothing,
            OccupationReason2 = PersonOccupationReason2.NobodyKnows,
            Occupation22 = PersonOccupation2.WorkingHard
        });
        _context.Add(new Person
        {
            PersonId = "BDAF4969-5406-4CDF-AD19-E34DF4837C34".G(),
            FirstName = "Sarah",
            LastName = "Connor",
            StatusStr = PersonStatus.Available,
            StatusInt = PersonStatus.Available,
            Sex = PersonSex.Female,
            Occupation = PersonOccupation.WorkingHard,
            OccupationReason = PersonOccupationReason.BecauseNeedsMoney,
            Sex2 = PersonSex2.Female,
            Occupation2 = PersonOccupation2.WorkingHard,
            OccupationReason2 = PersonOccupationReason2.BecauseNeedsMoney,
            Occupation22 = PersonOccupation2.DoingNothing
        });

        _context.Add(new PersonLegacy
        {
            PersonLegacyId = "A53673EC-D6F2-4368-BA3F-C258ED63762D".G(),
            FirstName = "Roman",
            LastName = "Turovskyy Legacy"
        });
        _context.Add(new PersonLegacy
        {
            PersonLegacyId = "5EC7C220-6FF6-4C6B-8987-AF8329B2DF2A".G(),
            FirstName = "Van",
            LastName = "Dam Legacy"
        });

        _context.Add(new HomeHealthAuthorization
        {
            AuthorizationNumber = "000",
            Status = AuthorizationStatus.Draft,
            Status2 = AuthorizationStatus2.Draft
        });

        _context.Add(new PacAuthorization
        {
            AuthorizationNumber = "111",
            AdmitDate = new DateTime(2020, 1, 1),
            Pacman = true,
            Status = AuthorizationStatus.Draft,
            Status2 = AuthorizationStatus2.Draft
        });
        _context.Add(new PacAuthorization
        {
            AuthorizationNumber = "222",
            AdmitDate = new DateTime(2020, 2, 2),
            Pacman = false,
            Status = AuthorizationStatus.Draft,
            Status2 = AuthorizationStatus2.Draft
        });

        _context.Add(new DmeAuthorization
        {
            AuthorizationNumber = "111",
            EquipmentCode = "c1",
            Status = AuthorizationStatus.Draft,
            Status2 = AuthorizationStatus2.Draft
        });
        _context.Add(new DmeAuthorization
        {
            AuthorizationNumber = "222",
            EquipmentCode = "c2",
            Status = AuthorizationStatus.Draft,
            Status2 = AuthorizationStatus2.Draft
        });

        await _context.SaveChangesAsync();

        // Introduce PlayingGames occupation which is not mapped in BE
        using (var conn = new SqlConnection(_context.Database.GetConnectionString()))
        {
            var cmd = new SqlCommand($"UPDATE dbo.Person set Occupation='{_playingGamesOccupatiodId}' WHERE PersonId='9135444A-7199-45C5-BFF4-671370176E61'", conn);
            var cmd2 = new SqlCommand($"UPDATE dbo.Person set Occupation2='{_playingGamesOccupatiodId}' WHERE PersonId='9135444A-7199-45C5-BFF4-671370176E61'", conn);
            conn.Open();
            // Commented, because as expected this leads to "we cannot read record from the database" problem, because enum in not mapped
            //cmd.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
        }
    }

    private void SeedEnums()
    {
        Product pacProduct = new Product { ShortName = "PAC" };
        Product dmeProduct = new Product { ShortName = "DME" };
        _context.Add(pacProduct);

        var enums = new List<ReferenceByProduct>();

        enums.Add(new ReferenceByProduct
        {
            Description = "Draft",
            SortOrder = 10,
            Entity = "BlogPost",
            Reference = "Status",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            Description = "On Review",
            SortOrder = 20,
            Entity = "BlogPost",
            Reference = "Status",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            Description = "Published",
            SortOrder = 30,
            Entity = "BlogPost",
            Reference = "Status",
            Product = pacProduct
        });


        enums.Add(new ReferenceByProduct
        {
            Description = "Unknown",
            SortOrder = 10,
            Entity = "Person",
            Reference = "Sex",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            Description = "Male",
            SortOrder = 20,
            Entity = "Person",
            Reference = "Sex",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            Description = "Female",
            SortOrder = 10,
            Entity = "Person",
            Reference = "Sex",
            Product = pacProduct
        });


        enums.Add(new ReferenceByProduct
        {
            Description = "Working Hard",
            SortOrder = 10,
            Entity = "Person",
            Reference = "Occupation",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            Description = "Doing Nothing",
            SortOrder = 20,
            Entity = "Person",
            Reference = "Occupation",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            ReferenceByProductId = _playingGamesOccupatiodId,
            Description = "Playing Games",
            SortOrder = 30,
            Entity = "Person",
            Reference = "Occupation",
            Product = pacProduct
        });


        enums.Add(new ReferenceByProduct
        {
            Description = "Nobody Knows",
            SortOrder = 10,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            Description = "Because Is Lazy",
            SortOrder = 20,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            Description = "Because Needs Money",
            SortOrder = 30,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });
        enums.Add(new ReferenceByProduct
        {
            Description = "Because Likes To Work",
            SortOrder = 40,
            Entity = "Person",
            Reference = "OccupationReason",
            Product = pacProduct
        });

        enums.Add(new ReferenceByProduct
        {
            Description = "Draft",
            SortOrder = 10,
            Entity = "Authorization",
            Reference = "Status",
            Product = dmeProduct
        });

        enums.Add(new ReferenceByProduct
        {
            Description = "Pending Approval",
            SortOrder = 20,
            Entity = "Authorization",
            Reference = "Status",
            Product = dmeProduct
        });

        enums.Add(new ReferenceByProduct
        {
            Description = "Canceled",
            SortOrder = 30,
            Entity = "Authorization",
            Reference = "Status",
            Product = dmeProduct
        });

        foreach (var e in enums)
            e.Code = e.Description.Replace(" ", "");

        _context.AddRange(enums);

        // Force save changes after seeding enums so data that depends on enums is available for the ReferenceByProductProvider
        _context.SaveChanges();
    }
}
