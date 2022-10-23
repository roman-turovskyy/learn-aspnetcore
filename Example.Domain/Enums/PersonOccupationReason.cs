namespace Example.Domain.Enums;

public enum PersonOccupationReason
{
    [ReferenceId("06EE2E89-E883-458D-85F8-F5FDED39C689")]
    NobodyKnows,

    [ReferenceId("A53454A5-72C0-4220-9A67-97AA2CFE7C4E")]
    BecauseNeedsMoney,

    [ReferenceId("42516B8A-B8DA-4665-8A3C-983972076597")]
    BecauseLikesToWork,

    [ReferenceId("2F342501-167B-407A-BB4C-3B422E2A7A85")]
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
