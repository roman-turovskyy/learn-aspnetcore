using Audit.EntityFramework;
using Example.Common.Messaging;
using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Example.Common.Database;

public class AuditingInterceptor : SaveChangesInterceptor
{
    private readonly DbContextHelper? _auditDbContextHelper;
    private IReadOnlyCollection<AuditRecord>? _collectedAuditRecords = null;
    private readonly IMessageBus _messageBus;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAuditDataProvider _auditDataProvider;

    public AuditingInterceptor(IMessageBus messageBus, IDateTimeProvider dateTimeProvider, IAuditDataProvider auditDataProvider)
    {
        _messageBus = messageBus;
        //_auditDbContextHelper = new DbContextHelper();
        _auditDbContextHelper = null;
        _dateTimeProvider = dateTimeProvider;
        _auditDataProvider = auditDataProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        _collectedAuditRecords = CollectAuditRecords(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        _collectedAuditRecords = CollectAuditRecords(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        SendAuditCommand();

        return base.SavedChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        SendAuditCommand();

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void SendAuditCommand()
    {
        if (_collectedAuditRecords is null || _collectedAuditRecords.Count == 0)
            return;

        List<ReferralAuditRecord> referralAuditRecords = _collectedAuditRecords.Where(i => i is IReferralAuditableEntity).Cast<ReferralAuditRecord>().ToList();
        if (referralAuditRecords.Any())
        {
            var referralCmd = new ReferralAuditAddCommand()
            {
                AuditRecords = referralAuditRecords
            };

            _messageBus.SendAsync(referralCmd);
        }

        List<AuditRecord> genericAuditRecords = _collectedAuditRecords.Except(referralAuditRecords).ToList();

        if (genericAuditRecords.Any())
        {
            var cmd = new AuditAddCommand()
            {
                AuditRecords = genericAuditRecords
            };

            _messageBus.SendAsync(cmd);
        }
    }

    private IReadOnlyCollection<AuditRecord> CollectAuditRecords(DbContext? context)
    {
        if (context == null)
            return new List<AuditRecord>().AsReadOnly();

        context.ChangeTracker.DetectChanges();

        if (_auditDbContextHelper != null)
            return CollectAuditRecordsUsingLegacyAuditPackage(context, _auditDbContextHelper);

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

    private IReadOnlyCollection<AuditRecord> CollectAuditRecordsUsingLegacyAuditPackage(DbContext context, DbContextHelper auditDbContextHelper)
    {
        EntityFrameworkEvent autidEvent = auditDbContextHelper.CreateAuditEvent(new AuditDbContextWrapper(context));

        List<EventEntry> auditableEntries = autidEvent.Entries.Where(e => e.Entity is IAuditableEntity).ToList();

        if (auditableEntries.Count == 0)
            return new List<AuditRecord>().AsReadOnly();

        DateTime auditDate = _dateTimeProvider.UtcNow;

        var collectedRecords = new List<AuditRecord>();

        foreach (EventEntry? entry in auditableEntries)
        {
            // Using regular cast since we are sure it will succeed
            IAuditableEntity entity = (IAuditableEntity)entry.Entity;

            var auditLogJson = new AuditRecord()
            {
                AuditAction = entry.Action,
                AuditData = entry.ToJson(),
                AuditDate = auditDate,
                // We can take ModifiedBy since there is an agreement that for newly created and modified entities this value must be set.
                AuditUser = GetAuditUser(entity),
                // In legacy auditing implementation this code looked like this: entry.EntityType.Name.
                EntityType = AuditHelper.GetEntityType(context.Entry(entity), context).Name,
                TablePk = GetEntityPrimaryKey(context, entity),
                TenantName = _auditDataProvider.TenantName,
                ProductId = _auditDataProvider.ProductId
            };
            collectedRecords.Add(auditLogJson);
        }

        return collectedRecords;
    }

    private string GetAuditUser(IAuditableEntity entity)
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

    private string GetEntityPrimaryKey(DbContext context, IAuditableEntity entity)
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
