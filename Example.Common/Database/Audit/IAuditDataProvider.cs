namespace Example.Common.Database;

/// <summary>
/// Implementation of this interface may depend on IHttpContextAccessor/IRequestContext and IAuditProductResolver
/// and get necessary data from those dependencies.
/// </summary>
public interface IAuditDataProvider
{
    string TenantName { get; }
    Guid ProductId { get; }
}
