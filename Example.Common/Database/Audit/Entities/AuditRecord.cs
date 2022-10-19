namespace Example.Common.Database;

public class AuditRecord
{
    public long AuditLogId { get; set; }
    public string EntityType { get; set; }
    public DateTime AuditDate { get; set; }
    public string AuditUser { get; set; }
    public string AuditData { get; set; }
    public string AuditAction { get; set; }
    public string TablePk { get; set; }
    public string TenantName { get; set; }
    public Guid ProductId { get; set; }
}
