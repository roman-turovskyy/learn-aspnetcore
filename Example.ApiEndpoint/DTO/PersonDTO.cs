using Example.Domain.Enums;

namespace Example.ApiEndpoint.DTO;

public class PersonDTO
{
    public Guid PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public PersonSex? Sex { get; set; }  // optional just for testing
    public PersonOccupation Occupation { get; set; }
    public PersonOccupationReason OccupationReason { get; set; }
    public string? Sex2 { get; set; }  // optional just for testing
    public string Occupation2 { get; set; }
    public string OccupationReason2 { get; set; }
    public PersonOccupation Occupation3 { get; set; }
}
