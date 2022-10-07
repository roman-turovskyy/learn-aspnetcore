using Example.Domain.Entities.Interfaces;

namespace Example.Domain.Entities;

public class EntityBase : ICreatedModifiedEntityFields
{
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; } = null!;
    public DateTime ModifiedDate { get; set; }
}
