namespace Example.Domain.Enums;

[ReferenceByProductEnum("DME", "Status", "Authorization")]
public enum Status
{
    Draft,

    PendingApproval,

    Canceled
}

public class Status2 : EnumReference
{
    public static readonly Status2 Draft = new Status2("Draft");
    public static readonly Status2 PendingApproval = new Status2("PendingApproval");
    public static readonly Status2 Canceled = new Status2("Canceled");

    public Status2(string value) : base(value, "DME", "Status", "Authorization")
    {
    }
}
