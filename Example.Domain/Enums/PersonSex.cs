namespace Example.Domain.Enums;

[ReferenceByProductEnum("PAC", "Sex", "Person")]
public enum PersonSex
{
    Unknown,

    Male,

    Female
}

public class PersonSex2 : EnumReference
{
    public static readonly PersonSex2 Unknown = new PersonSex2("Unknown");
    public static readonly PersonSex2 Male = new PersonSex2("Male");
    public static readonly PersonSex2 Female = new PersonSex2("Female");

    public PersonSex2(string value) : base(value, "PAC", "Sex", "Person")
    {
    }
}
