namespace Example.Domain.Entities;

/// <summary>
/// Differs from ICreatedModifiedEntityFields by these 2 points:
/// 1. "On" suffix instead of "Date" for Created/Modified timestamps.
/// 2. Modified fields are nullable.
/// </summary>
public interface ICreatedModifiedEntityFieldsLegacy
{
    string CreatedBy { get; set; }
    DateTime CreatedOn { get; set; }
    string? ModifiedBy { get; set; }
    DateTime? ModifiedOn{ get; set; }
}
