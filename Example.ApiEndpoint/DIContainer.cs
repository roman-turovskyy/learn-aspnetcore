using Example.Common.Database;
using Example.Common.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class DIContainer
{
    public static void Configure(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddHttpContextAccessor(); // https://stackoverflow.com/questions/37371264

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IUserProvider, UserProvider>();
        services.AddTransient<IMessageBus, MessageBus>();
        services.AddTransient<AuditingInterceptor>();
        services.AddTransient<SystemFieldsUpdateInterceptor>();

        services.AddDbContext<AppDbContext>(
            (sp, dbContextBuilder) =>
            {
                string connStr = builder.Configuration.GetConnectionString("ExampleDbConnStr");
                if (connStr == null)
                    throw new ConfigurationException("ExampleDbConnStr is missing.");

                dbContextBuilder
                    .UseSqlServer(connStr)
                    .AddInterceptors(
                        sp.GetRequiredService<AuditingInterceptor>(),
                        sp.GetRequiredService<SystemFieldsUpdateInterceptor>());
            });

        services.AddMediatR(typeof(ApplicationAssemblyMarkerClass));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
}
