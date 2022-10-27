namespace Example.Domain.Entities;

public abstract class AuthorizationByProduct : EntityBase
{
    public Guid AuthorizationByProductId { get; set; }
    public string AuthorizationNumber { get; set; }
    public AuthorizationStatus Status { get; set; }
    public AuthorizationStatus2 Status2 { get; set; }
}
