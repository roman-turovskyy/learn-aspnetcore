using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading;

namespace Example.Application.Tests.Helpers;

/// <summary>
/// In-memory database does not automatically set RowVersion column value.
/// This interceptor automatically sets RowVersion so developer of tests does not have to do this every time.
/// </summary>
internal class RowVersionSetInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetRowVersion(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetRowVersion(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetRowVersion(DbContext? context)
    {
        if (context == null)
            return;

        context.ChangeTracker.DetectChanges();

        const string rowVersionColName = "RowVersion";

        foreach (EntityEntry entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                foreach (var p in entry.CurrentValues.Properties)
                {
                    if (p.Name == rowVersionColName &&  p.ClrType == typeof(byte[]))
                    {
                        if (entry.CurrentValues[p.Name] == null)
                        {
                            // All we need to make EF validation stop complaining on field nullability is set non-null value.
                            // We do not check here if property in actually non-nullable, but this is good enough since our RowVersion properties are defined as non-nullable.
                            entry.CurrentValues[p.Name] = new byte[0];
                        }
                    }
                }
            }
        }
    }
}
