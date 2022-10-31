using Example.Common.Database;

namespace Example.Application.Tests.Fakes;

internal class FakeAuditDataProvider : IAuditDataProvider
{
    internal static readonly Guid TestProductId = new Guid("7B168ADD-6F35-43FA-AFEB-905E5174934A");

    public string TenantName => "TestTenant";

    public Guid ProductId => TestProductId;
}
