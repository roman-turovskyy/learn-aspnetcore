using Example.Application;
using Example.Common.Database;
using Example.Common.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class DIContainer
{
    public static void Configure(IServiceCollection services)
    {
        string? connStr = Environment.GetEnvironmentVariable("ExampleDbConnStr");
        if (connStr == null)
            throw new Exception("Environment variable ExampleDbConnStr is not defined.");

        services.AddTransient<IMessageBus, MessageBus>();
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        services.AddScoped((sp) =>
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                .UseSqlServer(connStr)
                                .UseAudit(serviceProvider)
                                .Options;
            return new AppDbContext(contextOptions);
        });

        services.AddMediatR(typeof(ApplicationAssemblyMarkerClass));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
}
