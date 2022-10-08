using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Example.Common.Database;

public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private const string ConsigurationSectionName = "DatabaseOptions";
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(DatabaseOptions options)
    {
        _configuration.GetSection(ConsigurationSectionName).Bind(options);
    }
}
