using Example.Common.Database;
using Example.DAL;
using Microsoft.EntityFrameworkCore;

namespace Example.Application;

public class AppDbContext : SampleDbContext
{
    public AppDbContext(DbContextOptions options, IAuditLegacyInterceptor auditLegacyInterceptor) : base(options, auditLegacyInterceptor)
    {
    }
}
