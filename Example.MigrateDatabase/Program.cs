﻿using Example.DAL;
using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        context.Add(new Person { PersonId = "AB01E1E6-8402-4DC7-8950-1EC85B0C7023".G(), FirstName = "Roman", LastName = "Turovskyy" });
        context.Add(new Person { PersonId = "9135444A-7199-45C5-BFF4-671370176E61".G(), FirstName = "Van", LastName = "Dam" });

        await context.SaveChangesAsync();
    }
}

static class StringExtensions
{
    public static Guid G(this string sGuid)
    {
        return new Guid(sGuid);
    }
}