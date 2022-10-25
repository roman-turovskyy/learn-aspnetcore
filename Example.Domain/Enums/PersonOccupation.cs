namespace Example.Domain.Enums;

[ReferenceByProductEnum("DME", "Occupation", "Person")]
public enum PersonOccupation
{
    WorkingHard,

    DoingNothing,

    [Code("CustomOccupationCode")]
    CustomCode
}

public class PersonOccupation2 : EnumReference
{
    public static readonly PersonOccupation2 WorkingHard = new PersonOccupation2("WorkingHard");
    public static readonly PersonOccupation2 DoingNothing = new PersonOccupation2("DoingNothing");

    // TODO: make private
    public PersonOccupation2(string value) : base(value, "DME", "Occupation", "Person")
    {
    }
}
