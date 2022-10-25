namespace Example.Domain.Entities;

public class Person : EntityBase, IAuditableEntity
{
    public Guid PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public PersonStatus? StatusInt { get; set; }  // of type int in  the database
    public PersonStatus? StatusStr { get; set; }  // if type string in the database
    public PersonSex? Sex { get; set; }  // optional just for testing
    public PersonOccupation Occupation { get; set; }
    public PersonOccupationReason OccupationReason { get; set; }
    public PersonSex2? Sex2 { get; set; }  // optional just for testing
    public PersonOccupation2 Occupation2 { get; set; }
    public PersonOccupation2 Occupation22 { get; set; }
    public PersonOccupationReason2 OccupationReason2 { get; set; }
    public byte[] RowVersion { get; set; }
}