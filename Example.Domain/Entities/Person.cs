namespace Example.Domain.Entities;

public class Person : EntityBase, IAuditableEntity
{
    public Guid PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public PersonSex? Sex { get; set; }  // optional just for testing
    public PersonOccupation Occupation { get; set; }
    public PersonOccupationReason OccupationReason { get; set; }
    public byte[] RowVersion { get; set; }
}