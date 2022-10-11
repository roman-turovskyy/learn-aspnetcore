namespace Example.Domain.Entities;

public class PersonLegacy : EntityBaseLegacy, IAuditableEntity
{
    public Guid PersonLegacyId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public byte[] RowVersion { get; set; } = null!;
}