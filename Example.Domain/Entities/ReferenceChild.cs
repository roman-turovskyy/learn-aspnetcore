using System.Runtime.Serialization;

namespace Example.Domain.Entities;

public class ReferenceChild
{
    public Guid ReferenceId { get; set; }

    public Guid ChildId { get; set; }

    [IgnoreDataMember]
    public ReferenceByProduct Reference { get; set; }

    public ReferenceByProduct Child { get; set; }
}