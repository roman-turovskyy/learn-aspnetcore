using Example.Common.Database;

namespace Example.ApiEndpoint;

public class AuditDataProvider : IAuditDataProvider
{
    public string TenantName => "SampleTenant";
    public Guid ProductId => Guid.Parse("00000000-A209-432E-80CA-448D20FBDB3A");
}
