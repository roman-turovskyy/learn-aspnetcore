using Example.DAL;
using Example.MigrateDatabase;
using Microsoft.EntityFrameworkCore;

await CreateDatabases();
await SeedData();

async Task SeedData()
{
    using (var context = GetDbContext())
    {
        await new DbDataSeeding(context).SeedData();
    }
}

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
