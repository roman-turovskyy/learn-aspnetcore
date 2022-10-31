using Example.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace Example.MigrateDatabase;

internal class NoOpAuditLegacyInterceptor : IAuditLegacyInterceptor
{
    public IReadOnlyCollection<AuditRecord> CollectAuditRecords(DbContext context)
    {
        return new AuditRecord[0];
    }

    public Task SendAuditCommand(IReadOnlyCollection<AuditRecord> collectedAuditRecords)
    {
        return Task.CompletedTask;
    }

    public void SetBuiltinAuditFields(DbContext context)
    {
    }
}
