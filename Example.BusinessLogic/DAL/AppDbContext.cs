using Example.DAL;
using Microsoft.EntityFrameworkCore;

namespace Example.Application;

public class AppDbContext : AdventureWorks2019Context, IAppDbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
