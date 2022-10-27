namespace Example.Domain.Enums;

[ReferenceByProductEnum("DME", "Status", "Authorization")]
public enum AuthorizationStatus
{
    Draft,

    PendingApproval,

    Canceled
}

public class AuthorizationStatus2 : EnumReference
{
    public static readonly AuthorizationStatus2 Draft = new AuthorizationStatus2("Draft");
    public static readonly AuthorizationStatus2 PendingApproval = new AuthorizationStatus2("PendingApproval");
    public static readonly AuthorizationStatus2 Canceled = new AuthorizationStatus2("Canceled");

    public AuthorizationStatus2(string value) : base(value, "DME", "Status", "Authorization")
    {
    }
}
