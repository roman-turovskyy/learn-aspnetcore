namespace Example.Domain.Enums;

public enum PersonOccupation
{
    [ReferenceId("614849E2-AB84-4459-A6EB-F34E5F7C7384")]
    WorkingHard,

    [ReferenceId("B32D5B99-103F-4358-A327-341BEFFD8513")]
    DoingNothing,

    NoMappedToDatabase
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
