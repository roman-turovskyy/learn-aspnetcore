namespace Example.Domain.Enums;

public enum PersonSex
{
    [ReferenceId("A2BD84B1-0F1A-48DD-B3B4-0BDA4E3E2EF0")]
    Unknown,

    [ReferenceId("E8AAB8B3-31E8-4BC9-975B-0A1991191F06")]
    Male,

    [ReferenceId("C4A48DB6-CB21-4602-B4E7-321AFB9864E1")]
    Female
}

public class PersonSex2 : EnumReference
{
    public static readonly PersonSex2 Unknown = new PersonSex2("Unknown");
    public static readonly PersonSex2 Male = new PersonSex2("Male");
    public static readonly PersonSex2 Female = new PersonSex2("Female");

    public PersonSex2(string value) : base(value, "DME", "Sex", "Person")
    {
    }
}
