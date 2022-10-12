using Audit.EntityFramework;
using Example.Common.Database.Audit;
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
    private CollectedAuditRecords? _collectedAuditRecords = null;
    private readonly IMessageBus _messageBus;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAuditDataProvider _auditDataProvider;

    public AuditingInterceptor(IMessageBus messageBus, IDateTimeProvider dateTimeProvider, IAuditDataProvider auditDataProvider)
    {
        _messageBus = messageBus;
        //_auditDbContextHelper = new DbContextHelper();
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
        if (_collectedAuditRecords is null || _collectedAuditRecords.Entities.Count == 0)
            return;

        var cmd = new AuditAddCommand()
        {
            AuditRecords = _collectedAuditRecords.Entities
        };

        _messageBus.SendAsync(cmd);
    }

    private CollectedAuditRecords CollectAuditRecords(DbContext? context)
    {
        if (context == null)
            return new CollectedAuditRecords();

        context.ChangeTracker.DetectChanges();

        if (_auditDbContextHelper != null)
            return CollectAuditRecordsUsingLegacyAuditPackage(context, _auditDbContextHelper);

        DateTime auditDate = _dateTimeProvider.UtcNow;

        CollectedAuditRecords collectedRecords = new CollectedAuditRecords();
        foreach (EntityEntry entry in context.ChangeTracker.Entries().Where(e => e.Entity is IAuditableEntity))
        {
            // Using regular cast since we are sure it will succeed
            IAuditableEntity entity = (IAuditableEntity)entry.Entity;
            bool modified = entry.State == EntityState.Modified || entry.State == EntityState.Added || entry.State == EntityState.Deleted;

            if (modified)
            {
                var audidEventEntry = new AuditEventEntry(entry, context);

                var auditLogJson = new AuditLogJson()
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

                collectedRecords.Entities.Add(auditLogJson);
            }
        }

        return collectedRecords;
    }

    private CollectedAuditRecords CollectAuditRecordsUsingLegacyAuditPackage(DbContext context, DbContextHelper auditDbContextHelper)
    {
        var collectedRecords = new CollectedAuditRecords();
        EntityFrameworkEvent autidEvent = auditDbContextHelper.CreateAuditEvent(new AuditDbContextWrapper(context));

        List<EventEntry> auditableEntries = autidEvent.Entries.Where(e => e.Entity is IAuditableEntity).ToList();

        if (auditableEntries.Count == 0)
            return collectedRecords;

        DateTime auditDate = _dateTimeProvider.UtcNow;

        foreach (EventEntry? entry in auditableEntries)
        {
            // Using regular cast since we are sure it will succeed
            IAuditableEntity entity = (IAuditableEntity)entry.Entity;

            var auditLogJson = new AuditLogJson()
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
            collectedRecords.Entities.Add(auditLogJson);
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
