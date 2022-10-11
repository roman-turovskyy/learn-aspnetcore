namespace Example.Domain.Entities;

public class Person : EntityBase, IAuditableEntity
{
    public Guid PersonId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public byte[] RowVersion { get; set; } = null!;
}