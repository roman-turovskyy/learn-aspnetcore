namespace Example.Domain.Common;

[AttributeUsage(AttributeTargets.Enum)]
public class ReferenceByProductEnum : Attribute
{
    public string ProductShortName {  get; }
    public string Reference { get; }  // Reference column from the database table
    public string Entity { get; }  // Entity column from the database table

    public ReferenceByProductEnum(string productShortName, string reference, string entity)
    {
        ProductShortName = productShortName;
        Reference = reference;
        Entity = entity;
    }
}
