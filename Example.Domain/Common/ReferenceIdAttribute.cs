namespace Example.Domain.Common;

[AttributeUsage(AttributeTargets.Field)]
public class ReferenceIdAttribute : Attribute
{
    public Guid ReferenceId { get; }

    public ReferenceIdAttribute(string referenceId)
    {
        ReferenceId = Guid.Parse(referenceId);
    }
}