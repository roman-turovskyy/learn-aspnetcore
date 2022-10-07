namespace Example.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow { get => DateTime.UtcNow; }
}
