namespace Example.Domain.Entities;

public class EntityBaseLegacy : ICreatedModifiedEntityFieldsLegacy
{
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedOn{ get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
