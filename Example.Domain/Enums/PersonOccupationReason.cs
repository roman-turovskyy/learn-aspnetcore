namespace Example.Domain.Enums;

[ReferenceByProductEnum("DME", "OccupationReason", "Person")]
public enum PersonOccupationReason
{
    NobodyKnows,

    BecauseNeedsMoney,

    BecauseLikesToWork,

    BecauseIsLazy
}


public class PersonOccupationReason2 : EnumReference
{
    public static readonly PersonOccupationReason2 NobodyKnows = new PersonOccupationReason2("NobodyKnows");
    public static readonly PersonOccupationReason2 BecauseNeedsMoney = new PersonOccupationReason2("BecauseNeedsMoney");
    public static readonly PersonOccupationReason2 BecauseLikesToWork = new PersonOccupationReason2("BecauseLikesToWork");
    public static readonly PersonOccupationReason2 BecauseIsLazy = new PersonOccupationReason2("BecauseIsLazy");

    public PersonOccupationReason2(string value) : base(value, "DME", "OccupationReason", "Person")
    {
    }
}
