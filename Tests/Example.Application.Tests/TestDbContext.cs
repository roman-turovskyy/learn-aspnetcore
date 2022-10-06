using Example.Common.Database;
using Example.Common.Messaging;
using Microsoft.EntityFrameworkCore;
using System;

namespace Example.Application.Tests;

internal static class TestDbContext
{
    public static AppDbContext Create()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .UseAudit(new Moq.Mock<IMessageBus>().Object)
           .Options;

        var context = new AppDbContext(options);
        return context;
    }
}
