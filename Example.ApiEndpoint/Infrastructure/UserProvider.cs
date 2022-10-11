namespace Example.ApiEndpoint;

public class UserProvider : IUserProvider
{
    private IHttpContextAccessor _httpContextAccessor;

    public UserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name
        ?? "Anonymous";
}
