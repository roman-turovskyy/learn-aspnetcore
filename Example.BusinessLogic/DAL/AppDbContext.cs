using Example.DAL;
using Microsoft.EntityFrameworkCore;

namespace Example.Application;

public class AppDbContext : AdventureWorks2019Context, IAppDbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AdventureWorks2019Context> options) : base(options)
    {
    }
}
