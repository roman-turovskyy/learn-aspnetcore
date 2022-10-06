using Example.DAL;
using Microsoft.EntityFrameworkCore;

namespace Example.Application;

public class AppDbContext : SampleDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
