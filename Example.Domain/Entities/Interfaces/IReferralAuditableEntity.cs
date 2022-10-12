namespace Example.Domain.Entities;

// All custom auditable entities must also inherit from IAuditableEntity
public interface IReferralAuditableEntity : IAuditableEntity
{
    int GetReferralId();
}
