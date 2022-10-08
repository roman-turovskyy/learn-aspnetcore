namespace Example.Common.Database;

public class AuditLogJson
{
    public long AuditLogId { get; set; }
    public string EntityType { get; set; } = null!;
    public DateTime AuditDate { get; set; }
    public string AuditUser { get; set; } = null!;
    public string AuditData { get; set; } = null!;
    public string AuditAction { get; set; } = null!;
    public string TablePk { get; set; } = null!;
    public string TenantName { get; set; } = null!;
    public Guid ProductId { get; set; }
}
