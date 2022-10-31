using Microsoft.EntityFrameworkCore;

namespace Example.Common.Database;

public interface IAuditLegacyInterceptor
{
    void SetBuiltinAuditFields(DbContext context);
    IReadOnlyCollection<AuditRecord> CollectAuditRecords(DbContext context);
    Task SendAuditCommand(IReadOnlyCollection<AuditRecord> collectedAuditRecords);
}