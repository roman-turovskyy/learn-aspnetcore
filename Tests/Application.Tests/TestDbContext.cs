using Application.DAL;
using DAL;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Tests
{
    internal static class TestDbContext
    {
        public static AppDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AdventureWorks2019Context>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            var context = new AppDbContext(options);
            return context;
        }
    }
}
