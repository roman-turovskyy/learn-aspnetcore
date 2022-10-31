using Example.Common;
using Example.Common.Database;
using Example.Common.Database.Audit;
using Example.Common.Database.Enums;
using Example.Common.Messaging;
using Example.DAL;
using Example.MigrateDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Example.Application.Tests.Helpers;

internal class TestDbContext
{
    public AppDbContext DbContext { get; private set; }
    public IDateTimeProvider TimeProvider { get; private set; }
    public IUserProvider UserProvider { get; private set; }
    public IAuditDataProvider AuditDataProvider { get; private set; }


    public TestDbContext(AppDbContext dbContext,
        IDateTimeProvider timeProvider,
        IUserProvider userProvider,
        IAuditDataProvider auditDataProvider)
    {
        DbContext = dbContext;
        TimeProvider = timeProvider;
        UserProvider = userProvider;
        AuditDataProvider = auditDataProvider;
    }

    public static TestDbContext Create(IDateTimeProvider? dateTimeProvider = null,
        IUserProvider? userProvider = null,
        IAuditDataProvider? auditDataProvider = null)
    {
        if (dateTimeProvider == null)
            dateTimeProvider = new FakeDateTimeProvider();

        if (userProvider == null)
            userProvider = new FakeUserProvider();

        if (auditDataProvider == null)
            auditDataProvider = new FakeAuditDataProvider();

        var builder = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

        if (!typeof(AppDbContext).IsAssignableTo(typeof(AuditableDbContextBase)))
            builder = builder.AddInterceptors(
                new BuiltinAuditFieldsUpdateInterceptor(dateTimeProvider, userProvider));

        builder = builder.AddInterceptors(new RowVersionSetInterceptor());

        var options = builder.Options;

        var context = new AppDbContext(options, new AuditLegacyInterceptor(dateTimeProvider, userProvider, auditDataProvider,
            new Moq.Mock<IMessageBus>().Object));

        var dataSeeder = new DbDataSeeding(context);
        dataSeeder.SeedEnums();

        ReferenceByProductProviderStaticHost.SetReferenceByProductProvider(
            new ReferenceByProductProvider(context, new MemoryCache(new MemoryCacheOptions())));

        return new TestDbContext(context, dateTimeProvider, userProvider, auditDataProvider);
    }

    public static implicit operator AppDbContext(TestDbContext testDbContext) => testDbContext.DbContext;
}
