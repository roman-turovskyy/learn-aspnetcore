using Example.Common;
using Example.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace Example.Application.Tests.Helpers;

internal class TestDbContext
{
    public AppDbContext DbContext { get; private set; }
    public IDateTimeProvider TimeProvider { get; private set; }
    public IUserProvider UserProvider { get; private set; }


    public TestDbContext(AppDbContext dbContext, IDateTimeProvider timeProvider, IUserProvider userProvider)
    {
        DbContext = dbContext;
        TimeProvider = timeProvider;
        UserProvider = userProvider;
    }

    public static TestDbContext Create(IDateTimeProvider? dateTimeProvider = null, IUserProvider? userProvider = null)
    {
        if (dateTimeProvider == null)
            dateTimeProvider = new FakeDateTimeProvider();

        if (userProvider == null)
            userProvider = new FakeUserProvider();

        var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .AddInterceptors(
                new SystemFieldsUpdateInterceptor(dateTimeProvider, userProvider),
                new RowVersionSetInterceptor())
           .Options;

        var context = new AppDbContext(options);

        return new TestDbContext(context, dateTimeProvider, userProvider);
    }

    public static implicit operator AppDbContext(TestDbContext testDbContext) => testDbContext.DbContext;
}
