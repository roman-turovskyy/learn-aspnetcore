namespace Example.Common.Database;

public class AuditingException : Exception
{
    public AuditingException(string? message) : base(message)
    {
    }
}
