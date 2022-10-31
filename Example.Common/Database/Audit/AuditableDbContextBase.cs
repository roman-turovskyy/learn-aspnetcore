using Microsoft.EntityFrameworkCore;

namespace Example.Common.Database;

public class AuditableDbContextBase : DbContext
{
    private readonly IAuditLegacyInterceptor _auditLegacyInterceptor;

    public AuditableDbContextBase(DbContextOptions options, IAuditLegacyInterceptor auditLegacyInterceptor) : base(options)
    {
        _auditLegacyInterceptor = auditLegacyInterceptor;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        _auditLegacyInterceptor.SetBuiltinAuditFields(this);
        IReadOnlyCollection<AuditRecord> auditRecords = _auditLegacyInterceptor.CollectAuditRecords(this);

        int saveChangesResult = base.SaveChanges(acceptAllChangesOnSuccess);

        _auditLegacyInterceptor.SendAuditCommand(auditRecords);

        return saveChangesResult;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        _auditLegacyInterceptor.SetBuiltinAuditFields(this);
        IReadOnlyCollection<AuditRecord> auditRecords = _auditLegacyInterceptor.CollectAuditRecords(this);

        int saveChangesResult = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        await _auditLegacyInterceptor.SendAuditCommand(auditRecords);

        return saveChangesResult;
    }
}
