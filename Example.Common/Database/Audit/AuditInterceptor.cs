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
    private readonly DbContextHelper _auditDbContextHelper;
    private CollectedAuditRecords? _collectedAuditRecords = null;
    private readonly IMessageBus _messageBus;
    private readonly IDateTimeProvider _dateTimeProvider;
    // TODO: is this a good idea to encapsulate different dependencies into this single interface?
    private readonly IAuditDataProvider _auditDataProvider;

    public AuditingInterceptor(IMessageBus messageBus, IDateTimeProvider dateTimeProvider, IAuditDataProvider auditDataProvider)
    {
        _messageBus = messageBus;
        _auditDbContextHelper = new DbContextHelper();
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

        Console.WriteLine($"Sending AuditMessage:\r\n{_collectedAuditRecords}");
    }

    private CollectedAuditRecords CollectAuditRecords(DbContext? context)
    {
        var collectedRecords = new CollectedAuditRecords();

        if (context == null)
            return collectedRecords;

        context.ChangeTracker.DetectChanges();

        EntityFrameworkEvent autidEvent = _auditDbContextHelper.CreateAuditEvent(new AuditDbContextWrapper(context));

        var auditableEntries = autidEvent.Entries.Where(e => e.Entity is IAuditableEntity).ToList();

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
                AuditUser = entity.ModifiedBy,
                // In legacy auditing implementation this code looked like this: entry.EntityType.Name.
                EntityType = entry.Entity.GetType().Name,
                TablePk = GetEntityPrimaryKey(context, entry),
                TenantName = _auditDataProvider.TenantName,
                ProductId = _auditDataProvider.ProductId

            };
            collectedRecords.Entities.Add(auditLogJson);
        }

        context.ChangeTracker.DetectChanges();

        foreach (EntityEntry entry in context.ChangeTracker.Entries())
        {
            bool auditable = entry.Entity is IAuditableEntity;
            bool modified = entry.State == EntityState.Modified || entry.State == EntityState.Added || entry.State == EntityState.Deleted;

            if (auditable && modified)
            {
                collectedRecords.Entities.Add(new AuditLogJson { AuditAction = entry.State.ToString() });
            }
        }

        return collectedRecords;
    }

    private string GetEntityPrimaryKey(DbContext context, EventEntry eventEntry)
    {
        var entry = context.Entry(eventEntry.Entity);

        IKey? primaryKey = entry.Metadata.FindPrimaryKey();
        if (primaryKey == null)
            throw new AuditingException($"Auditable entity {eventEntry.Name} must have primary key defined");

        // In legacy auditing implementation this code looked like this: eventEntry.PrimaryKey.First().Value.ToString()
        object? primaryKeyValue = primaryKey.Properties.Select(x => x.PropertyInfo?.GetValue(eventEntry.Entity)).FirstOrDefault();
        if (primaryKeyValue == null)
            throw new AuditingException($"Auditable entity {eventEntry.Name} must have non-empty primary key");

        // Bang to calm down compiler
        return primaryKeyValue.ToString()!;
    }
}
