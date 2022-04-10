using Application;
using Application.DAL;
using Application.Services.Decorators;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class DIContainer
{
    public static void Configure(IServiceCollection services)
    {
        string? connStr = Environment.GetEnvironmentVariable("AdventureWorks2019ConnStr");
        if (connStr == null)
            throw new Exception("Environment variable AdventureWorks2019ConnStr is not defined.");

        services.AddDbContext<AppDbContext>(builder => builder.UseSqlServer(connStr));
        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddMediatR(typeof(ApplicationAssemblyMarkerClass));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
}
