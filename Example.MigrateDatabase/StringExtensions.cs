namespace Example.MigrateDatabase;

internal static class StringExtensions
{
    public static Guid G(this string sGuid)
    {
        return new Guid(sGuid);
    }
}
