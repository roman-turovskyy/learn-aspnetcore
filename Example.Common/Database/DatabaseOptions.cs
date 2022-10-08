namespace Example.Common.Database;

public class DatabaseOptions
{
    public int MaxRetryCount { get; set; }
    public int CommandTimeout { get; set; }
    public bool EnableDetailedError { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
}
