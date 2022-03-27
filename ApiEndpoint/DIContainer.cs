using Application.DAL;
using Application.Services;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

public static class DIContainer
{
    public static void Configure(IServiceCollection services)
    {
        string? connStr = Environment.GetEnvironmentVariable("AdventureWorks2019ConnStr");
        if (connStr == null)
            throw new Exception("Environment variable AdventureWorks2019ConnStr is not defined.");

        services.AddDbContext<AppDbContext>(builder => builder.UseSqlServer(connStr));
        services.AddScoped<IQueryHandler<GetPersonQuery, Person>,
            GetPersonQueryHandler>();
        services.AddScoped<IQueryHandler<GetPersonListQuery, IList<Person>>,
            GetPersonListQueryHandler>();
        services.AddScoped<ICommandHandler<CreatePersonCommand>,
            CreatePersonCommandHandler>();
    }
}
