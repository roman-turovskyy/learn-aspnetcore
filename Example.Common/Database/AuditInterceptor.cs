using Example.Common.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Example.Common.Database;

public class AuditingInterceptor : SaveChangesInterceptor
{
    private CollectedAuditRecords? _collectedAuditRecords = null;
    private IMessageBus _messageBus;

    public AuditingInterceptor(IMessageBus messageBus)
    {
        _messageBus = messageBus;
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

    private static CollectedAuditRecords CollectAuditRecords(DbContext? context)
    {
        var collectedRecords = new CollectedAuditRecords();

        if (context == null)
            return collectedRecords;

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
}
