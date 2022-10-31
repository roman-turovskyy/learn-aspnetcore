using Example.Common.Messaging;
using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Example.Common.Database;

public class AuditLegacyInterceptor : IAuditLegacyInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserProvider _userProvider;
    private readonly IAuditDataProvider _auditDataProvider;
    private readonly IMessageBus _messageBus;

    public AuditLegacyInterceptor(IDateTimeProvider dateTimeProvider, IUserProvider userProvider, IAuditDataProvider auditDataProvider, IMessageBus messageBus)
    {
        _dateTimeProvider = dateTimeProvider;
        _userProvider = userProvider;
        _auditDataProvider = auditDataProvider;
        _messageBus = messageBus;
    }

    public void SetBuiltinAuditFields(DbContext context)
    {
        // Make a local variable so all entities will get the same value
        DateTime utcNow = _dateTimeProvider.UtcNow;
        string userName = _userProvider.UserName;

        // For scenarios when caller does not want us to update CreatedBy/ModifiedBy,
        // e.g. because he manually set these fields to some custom values.
        const string dontUpdateUserSentinelValue = "";

        foreach (EntityEntry entry in context.ChangeTracker.Entries())
        {
            bool modified = entry.State == EntityState.Modified || entry.State == EntityState.Added;

            if (modified && entry.Entity is ICreatedModifiedEntityFields entityFields)
            {
                if (entry.State == EntityState.Added)
                {
                    entityFields.CreatedDate = utcNow;

                    if (userName != dontUpdateUserSentinelValue)
                        entityFields.CreatedBy = userName;
                }

                entityFields.ModifiedDate = utcNow;

                if (userName != dontUpdateUserSentinelValue)
                    entityFields.ModifiedBy = userName;
            }
        }
    }

    public IReadOnlyCollection<AuditRecord> CollectAuditRecords(DbContext context)
    {
        DateTime auditDate = _dateTimeProvider.UtcNow;

        var collectedRecords = new List<AuditRecord>();
        foreach (EntityEntry entry in context.ChangeTracker.Entries().Where(e => e.Entity is IAuditableEntity))
        {
            // Using regular cast since we are sure it will succeed
            IAuditableEntity entity = (IAuditableEntity)entry.Entity;
            bool modified = entry.State == EntityState.Modified || entry.State == EntityState.Added || entry.State == EntityState.Deleted;

            if (modified)
            {
                var audidEventEntry = new AuditEventEntry(entry, context);

                var auditRecord = new AuditRecord()
                {
                    AuditAction = audidEventEntry.Action,
                    AuditData = audidEventEntry.ToJson(),
                    AuditDate = auditDate,
                    // We can take ModifiedBy since there is an agreement that for newly created and modified entities this value must be set.
                    AuditUser = GetAuditUser(entity),
                    // In legacy auditing implementation this code looked like this: entry.EntityType.Name.
                    EntityType = AuditHelper.GetEntityType(entry, context).Name,
                    TablePk = GetEntityPrimaryKey(context, entity),
                    TenantName = _auditDataProvider.TenantName,
                    ProductId = _auditDataProvider.ProductId
                };

                if (entry.Entity is IReferralAuditableEntity referralAuditable)
                {
                    // Pretty naive implementation, need to be improved
                    auditRecord = new ReferralAuditRecord()
                    {
                        AuditAction = auditRecord.AuditAction,
                        AuditData = auditRecord.AuditData,
                        AuditDate = auditRecord.AuditDate,
                        AuditUser = auditRecord.AuditUser,
                        EntityType = auditRecord.EntityType,
                        TablePk = auditRecord.TablePk,
                        TenantName = auditRecord.TenantName,
                        ProductId = auditRecord.ProductId,

                        // This is how we plan to get ReferralId, logic will be encapsulated inside GetReferralId()
                        ReferralId = referralAuditable.GetReferralId()
                    };
                }

                collectedRecords.Add(auditRecord);
            }
        }

        return collectedRecords.AsReadOnly();
    }

    public async Task SendAuditCommand(IReadOnlyCollection<AuditRecord> collectedAuditRecords)
    {
        if (collectedAuditRecords is null || collectedAuditRecords.Count == 0)
            return;

        List<ReferralAuditRecord> referralAuditRecords = collectedAuditRecords.Where(i => i is IReferralAuditableEntity).Cast<ReferralAuditRecord>().ToList();
        if (referralAuditRecords.Any())
        {
            var referralCmd = new ReferralAuditAddCommand()
            {
                AuditRecords = referralAuditRecords
            };

            await _messageBus.SendAsync(referralCmd);
        }

        List<AuditRecord> genericAuditRecords = collectedAuditRecords.Except(referralAuditRecords).ToList();

        if (genericAuditRecords.Any())
        {
            var cmd = new AuditAddCommand()
            {
                AuditRecords = genericAuditRecords
            };

            await _messageBus.SendAsync(cmd);
        }
    }

    private static string GetAuditUser(IAuditableEntity entity)
    {
        if (entity is ICreatedModifiedEntityFields e)
        {
            return e.ModifiedBy;
        }
        else if (entity is ICreatedModifiedEntityFieldsLegacy eLegacy)
        {
            return eLegacy.ModifiedBy ?? eLegacy.CreatedBy;
        }

        throw new AuditingException($"IAuditableEntity entity {entity.GetType().FullName} must implement either " +
            $"ICreatedModifiedEntityFields or ICreatedModifiedEntityFieldsLegacy.");
    }

    private static string GetEntityPrimaryKey(DbContext context, IAuditableEntity entity)
    {
        EntityEntry<IAuditableEntity> entry = context.Entry(entity);

        IKey? primaryKey = entry.Metadata.FindPrimaryKey();
        if (primaryKey == null)
            throw new AuditingException($"Auditable entity {entry.GetType().Name} must have primary key defined.");

        // In legacy auditing implementation this code looked like this: eventEntry.PrimaryKey.First().Value.ToString()
        List<object?> primaryKeyValues = primaryKey.Properties.Select(x => x.PropertyInfo?.GetValue(entity)).ToList();

        if (primaryKeyValues.Count != 1)
            throw new AuditingException($"Auditable entity {entry.GetType().Name} cannot have composite primary key (not supported in the current implementation).");

        string? primaryKeyValue = primaryKeyValues?.FirstOrDefault()?.ToString();

        if (primaryKeyValue == null)
            throw new AuditingException($"Auditable entity {entry.GetType().Name} must have non-empty primary key.");

        return primaryKeyValue;
    }
}
