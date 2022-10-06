namespace Example.Common.Database;

public class CollectedAuditRecords
{
    public ICollection<AuditLogJson> Entities { get; } = new List<AuditLogJson>();

    public override string ToString()
    {
        return String.Join("\r\n", Entities);
    }
}
