using Example.Common;

namespace Example.Application.Tests.Fakes;

internal class FakeUserProvider : IUserProvider
{
    private string _userName;

    public FakeUserProvider(string? userName = null)
    {
        _userName = userName ?? "TestUser";
    }

    public string UserName => _userName;
}
