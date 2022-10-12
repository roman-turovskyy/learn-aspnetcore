using Audit.EntityFramework;
using Audit.EntityFramework.ConfigurationApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Example.Common.Database;

/// <summary>
/// Many methods from this helper class inspired by the Audit.EntityFramework.DbContextHelper implementation
/// because we need a backward compatible implementation with this auditing library.
/// </summary>
public static class AuditHelper
{
    internal static string GetStateName(EntityState state)
    {
        switch (state)
        {
            case EntityState.Added:
                return "Insert";
            case EntityState.Modified:
                return "Update";
            case EntityState.Deleted:
                return "Delete";
            default:
                return "Unknown";
        }
    }

    /// <summary>
    /// Implementation inspired by Audit.EntityFramework.Providers.EntityFrameworkDataProvider
    /// </summary>
    internal static Type GetEntityType(EntityEntry entry, DbContext localDbContext)
    {
        IReadOnlyEntityType? definingType = entry.Metadata.FindOwnership()?.DeclaringEntityType ?? localDbContext.Model.FindEntityType(entry.Metadata.Name);
        Type? type = definingType?.ClrType;
        if (type != null)
            return type;

        Type? entryType = GetTypeNoProxy(entry.Entity.GetType());
        if (entryType != null)
        {
            definingType = localDbContext.Model.FindEntityType(entryType);
            type = definingType?.ClrType;
            if (type != null)
                return type;
        }

        return entry.Entity.GetType();
    }

    /// <summary>
    /// Implementation inspired by Audit.EntityFramework.Providers.EntityFrameworkDataProvider
    /// </summary>
    private static Type? GetTypeNoProxy(Type type)
    {
        if (type == null)
            return null;

        if (type.FullName != null && type.FullName.StartsWith("Castle.Proxies."))
            return type.GetTypeInfo().BaseType;

        return type;
    }

    /// <summary>
    /// Gets the validation results, return NULL if there are no validation errors.
    /// Implementation taken from Audit.EntityFramework.DbContextHelper.GetValidationResults
    /// </summary>
    internal static List<ValidationResult>? GetValidationResults(object entity)
    {
        var validationContext = new ValidationContext(entity);
        var validationResults = new List<ValidationResult>();
        bool valid = Validator.TryValidateObject(entity, validationContext, validationResults, true);
        return valid ? null : validationResults;
    }

    /// <summary>
    /// Implementation taken from Audit.EntityFramework.DbContextHelper.GetEntityName
    /// </summary>
    internal static AuditEntityName? GetEntityName(EntityEntry entry, DbContext context)
    {
        var definingType = GetDefiningType(context, entry);
        if (definingType == null)
            return null;

        return new AuditEntityName()
        {
            Table = definingType.GetTableName(),
            Schema = definingType.GetSchema()
        };
    }

    internal static IReadOnlyEntityType? GetDefiningType(DbContext dbContext, EntityEntry entry)
    {
        return entry.Metadata.FindOwnership()?.DeclaringEntityType ?? dbContext.Model.FindEntityType(entry.Metadata.Name);
    }

    /// <summary>
    /// Implementation taken from Audit.EntityFramework.DbContextHelper.GetPrimaryKey
    /// </summary>
    internal static Dictionary<string, object> GetPrimaryKey(DbContext dbContext, EntityEntry entry)
    {
        var result = new Dictionary<string, object>();
        foreach (var prop in entry.Properties.Where(p => p.Metadata.IsPrimaryKey()))
        {
            result.Add(GetColumnName(prop.Metadata), prop.CurrentValue!); // bang to calm down compiler
        }
        return result;
    }

    internal static string GetColumnName(IProperty prop)
    {
        var storeObjectIdentifier = StoreObjectIdentifier.Create(prop.DeclaringEntityType, StoreObjectType.Table);

        string? columnName = null;

        if (storeObjectIdentifier.HasValue)
            columnName = prop.GetColumnName(storeObjectIdentifier.Value);

        if (columnName is null)
            columnName = prop.GetDefaultColumnBaseName();

        return columnName;
        // For EF Core 3
        // return prop.GetColumnName();
    }
}
