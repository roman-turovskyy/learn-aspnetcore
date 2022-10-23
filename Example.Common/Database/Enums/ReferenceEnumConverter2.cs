using Example.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Example.Common.Database.Enums;

public static class ReferenceEnumConverterCache
{
    private static IReferenceByProductProvider _referenceByProductProvider;
    private static List<ReferenceByProductCommon>? _referenceByProductCache = null;

    public static List<ReferenceByProductCommon> ReferenceByProductCache
    {
        get
        {
            EnsureCache();

            return _referenceByProductCache!;
        }
    }

    public static void SetReferenceByProductProvider(IReferenceByProductProvider referenceByProductProvider)
    {
        _referenceByProductProvider = referenceByProductProvider;
    }

    private static void EnsureCache()
    {
        _referenceByProductCache ??= _referenceByProductProvider.GetRefrences();
    }

}

public class ReferenceEnumConverter2<T> : ValueConverter<T, Guid>
        where T : EnumReference
{
    private static readonly Expression<Func<T, Guid>> convertToProviderExpression = x => ToGuid(x);
    private static readonly Expression<Func<Guid, T>> convertFromProviderExpression = x => ToEnum(x);

    public ReferenceEnumConverter2()
         : base(convertToProviderExpression, convertFromProviderExpression)
    {
    }

    private static Guid ToGuid(EnumReference value)
    {
        ReferenceByProductCommon? r = ReferenceEnumConverterCache.ReferenceByProductCache.FirstOrDefault(x => x.Code == value.Value
            && x.Entity == value.Entity
            && x.Reference == value.Reference
            && x.ProductShortName == value.ProductShortName);

        if (r == null )
            // TODO: better Exception
            throw new ArgumentException($"Can't convert value {(T)value} to Guid.");

        return r.Id;
    }

    private static T ToEnum(Guid referenceId)
    {
        ReferenceByProductCommon? r = ReferenceEnumConverterCache.ReferenceByProductCache.FirstOrDefault(x => x.Id == referenceId);

        if (r == null)
            // TODO: better Exception
            throw new ArgumentException($"Can't convert value {referenceId} to Enum.");

        // TODO: here we assume Entity, Reference, ProductShortName matches data from the database. Maybe better to ensure?
        return (T)Activator.CreateInstance(typeof(T), new[] { r.Code })!;
    }
}
