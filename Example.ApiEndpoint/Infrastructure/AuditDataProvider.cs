using Example.Common.Database;

namespace Example.ApiEndpoint;

public class AuditDataProvider : IAuditDataProvider
{
    public string TenantName { get => "SampleTenant"; }
    public Guid ProductId { get => Guid.Parse("00000000-A209-432E-80CA-448D20FBDB3A"); }
}
