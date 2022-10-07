namespace Example.Common
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}