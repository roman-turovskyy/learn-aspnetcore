using Example.Common.Database;
using Example.Common.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

        services.AddMediatR(typeof(ApplicationAssemblyMarkerClass));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        AddDbContext(builder, services);
    }

    private static void AddDbContext(WebApplicationBuilder builder, IServiceCollection services)
    {
        builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

        services.AddDbContext<AppDbContext>(
            (sp, dbContextBuilder) =>
            {
                DatabaseOptions databaseOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;

                // I store connection string in environment variable with name SQLCONNSTR_ExampleDbConnStr
                string connStr = builder.Configuration.GetConnectionString("ExampleDbConnStr");
                if (connStr == null)
                    throw new ConfigurationException("ExampleDbConnStr is missing.");

                dbContextBuilder
                    .UseSqlServer(connStr, sqlServerOptAction =>
                    {
                        sqlServerOptAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
                        sqlServerOptAction.CommandTimeout(databaseOptions.CommandTimeout);
                    })
                    .AddInterceptors(
                        sp.GetRequiredService<AuditingInterceptor>(),
                        sp.GetRequiredService<SystemFieldsUpdateInterceptor>())
                    .EnableDetailedErrors(databaseOptions.EnableDetailedError)
                    .EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            });
    }
}
