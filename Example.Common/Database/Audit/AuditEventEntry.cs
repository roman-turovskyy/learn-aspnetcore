using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Example.Common.Database;

/// <summary>
/// This class is constructed so ToJson() is backward-compatible with Audit.EntityFramework.EventEntry.
/// Tested on our existing audit records in DEV and QA.
/// </summary>
internal class AuditEventEntry
{
    public AuditEventEntry(EntityEntry entry, DbContext context)
    {
        var entityName = AuditHelper.GetEntityName(entry, context);

        Action = AuditHelper.GetStateName(entry.State);
        Schema = entityName?.Schema;
        Table = entityName?.Table;
        PrimaryKey = AuditHelper.GetPrimaryKey(context, entry);
        Changes = entry.State == EntityState.Modified ? GetChanges(entry) : null;
        ColumnValues = GetColumnValues(entry);
        Valid = AuditHelper.GetValidationResults(entry) == null;
    }

    public string? Schema { get; set; }
    public string? Table { get; set; }
    public IDictionary<string, object> PrimaryKey { get; set; }
    public string Action { get; set; }
    public List<EventEntryChange>? Changes { get; set; }
    public IDictionary<string, object> ColumnValues { get; set; }
    public bool Valid { get; set; }

    public string ToJson()
    {
        // NullValueHandling.Ignore to ignore properties with null value
        return JsonConvert.SerializeObject(this, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
    }

    private List<EventEntryChange> GetChanges(EntityEntry entry)
    {
        var result = new List<EventEntryChange>();
        var props = entry.Metadata.GetProperties();
        foreach (var prop in props)
        {
            PropertyEntry propEntry = entry.Property(prop.Name);
            if (propEntry.IsModified)
            {
                result.Add(new EventEntryChange()
                {
                    ColumnName = AuditHelper.GetColumnName(prop),
                    NewValue = propEntry.CurrentValue,
                    OriginalValue = propEntry.OriginalValue
                });
            }
        }
        return result;
    }

    /// <summary>
    /// Gets the column values for an insert/delete operation.
    /// </summary>
    private Dictionary<string, object> GetColumnValues(EntityEntry entry)
    {
        var result = new Dictionary<string, object>();
        var props = entry.Metadata.GetProperties();

        foreach (var prop in props)
        {
            PropertyEntry propEntry = entry.Property(prop.Name);
            object? value = entry.State != EntityState.Deleted ? propEntry.CurrentValue : propEntry.OriginalValue;

            result.Add(AuditHelper.GetColumnName(prop), value!); // bang to calm down compiler
        }

        return result;
    }
}
