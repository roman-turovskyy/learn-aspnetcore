using Audit.Core;
using Audit.EntityFramework;
using Audit.EntityFramework.ConfigurationApi;
using Microsoft.EntityFrameworkCore;

namespace Example.Common.Database;

/// <summary>
/// This stub implementation was created to make Audit.EntityFramework happy.
/// </summary>
internal class AuditDbContextWrapper : IAuditDbContext
{
    private DbContext _dbContext;

    public AuditDbContextWrapper(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string AuditEventType { get; set; }
    public bool AuditDisabled { get; set; }
    // It is important to set this property to true, so EventEntry.Entity will be populated.
    public bool IncludeEntityObjects { get; set; } = true;
    public bool ExcludeValidationResults { get; set; }
    public AuditOptionMode Mode { get; set; }
    public AuditDataProvider AuditDataProvider { get; set; }
    public IAuditScopeFactory AuditScopeFactory { get; set; }
    public Dictionary<string, object> ExtraFields { get; } = new Dictionary<string, object>();
    public DbContext DbContext => _dbContext;
    public Dictionary<Type, EfEntitySettings> EntitySettings { get; set; }
    public bool ReloadDatabaseValues { get; set; }
    public bool ExcludeTransactionId { get; set; }
    public bool EarlySavingAudit { get; set; }

    public void OnScopeCreated(IAuditScope auditScope)
    {
    }

    public void OnScopeSaved(IAuditScope auditScope)
    {
    }

    public void OnScopeSaving(IAuditScope auditScope)
    {
    }
}