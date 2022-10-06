using Example.Application;
using Example.Common.Database;
using Example.Common.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class DIContainer
{
    public static void Configure(IServiceCollection services)
    {
        string? connStr = Environment.GetEnvironmentVariable("AdventureWorks2019ConnStr");
        if (connStr == null)
            throw new Exception("Environment variable AdventureWorks2019ConnStr is not defined.");

        services.AddTransient<IMessageBus, MessageBus>();
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        services.AddScoped<AppDbContext>((sp) =>
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
