using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Example.Common.Database;

public class SystemFieldsUpdateInterceptor : SaveChangesInterceptor
{
    private IDateTimeProvider _dateTimeProvider;
    private IUserProvider _userProvider;

    public SystemFieldsUpdateInterceptor(IDateTimeProvider dateTimeProvider, IUserProvider userProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _userProvider = userProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetSystemFields(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetSystemFields(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetSystemFields(DbContext? context)
    {
        if (context == null)
            return;

        context.ChangeTracker.DetectChanges();

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
}
