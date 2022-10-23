namespace Example.Domain.Enums;

public class EnumReference
{
    public string Value { get; }
    public string ProductShortName { get; }
    public string Reference { get; }
    public string Entity { get; }


    protected EnumReference(string value)
    {
        Value = value;
    }

    // TODO: make protected
    public EnumReference(string value, string productShortName, string reference, string entity)
    {
        Value = value;
        ProductShortName = productShortName;
        Reference = reference;
        Entity = entity;
    }

    public override string ToString()
    {
        return Value;
    }
}
