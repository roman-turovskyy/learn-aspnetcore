namespace Example.Domain.Entities;

public class EntityBase : ICreatedModifiedEntityFields
{
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
}
