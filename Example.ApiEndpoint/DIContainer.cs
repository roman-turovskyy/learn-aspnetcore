using Example.Common.Database;
using Example.Common.Database.Audit;
using Example.Common.Database.Enums;
using Example.Common.Messaging;
using Example.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

public static class DIContainer
{
    public static void Configure(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddHttpContextAccessor(); // https://stackoverflow.com/questions/37371264
        services.AddMemoryCache(); // Because we are using IMemoryCache

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IUserProvider, UserProvider>();
        services.AddTransient<IMessageBus, MessageBus>();

        services.AddTransient<AuditingInterceptor>();
        services.AddTransient<IAuditDataProvider, AuditDataProvider>();
        services.AddTransient<IAuditLegacyInterceptor, AuditLegacyInterceptor>();

        services.AddTransient<BuiltinAuditFieldsUpdateInterceptor>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ApplicationAssemblyMarkerClass).Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        AddDbContext(builder, services);

        var sp = services.BuildServiceProvider();

        ReferenceByProductProviderStaticHost.SetReferenceByProductProvider(
            new ReferenceByProductProvider(sp.GetRequiredService<AppDbContext>(), sp.GetRequiredService<IMemoryCache>()));
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
                    //.AddInterceptors(
                    //    sp.GetRequiredService<BuiltinAuditFieldsUpdateInterceptor>(),
                    //    sp.GetRequiredService<AuditingInterceptor>())
                    .EnableDetailedErrors(databaseOptions.EnableDetailedError)
                    .EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            });
    }
}
