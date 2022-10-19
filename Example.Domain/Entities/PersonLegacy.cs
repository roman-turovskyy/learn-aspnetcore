namespace Example.Domain.Entities;

public class PersonLegacy : EntityBaseLegacy, IReferralAuditableEntity
{
    public Guid PersonLegacyId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[] RowVersion { get; set; }

    public int GetReferralId()
    {
        return 123;
    }
}