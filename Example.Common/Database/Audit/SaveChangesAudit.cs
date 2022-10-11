namespace Example.Common.Database.Audit;

public class CollectedAuditRecords
{
    public ICollection<AuditLogJson> Entities { get; } = new List<AuditLogJson>();

    public override string ToString()
    {
        return string.Join("\r\n", Entities);
    }
}
