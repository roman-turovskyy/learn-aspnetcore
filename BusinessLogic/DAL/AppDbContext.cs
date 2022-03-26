using DAL;
using Microsoft.EntityFrameworkCore;

namespace Application.DAL
{
    public class AppDbContext : AdventureWorks2019Context
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AdventureWorks2019Context> options) : base(options)
        {
        }
    }
}
