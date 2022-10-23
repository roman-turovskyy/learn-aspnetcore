using Example.Domain.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;
using System.Reflection;

namespace Example.Common.Database.Enums;

public class ReferenceEnumConverter<T> : ValueConverter<T, Guid>
        where T : struct, Enum
{
    private static readonly Expression<Func<T, Guid>> convertToProviderExpression = x => ToGuid(x);
    private static readonly Expression<Func<Guid, T>> convertFromProviderExpression = x => ToEnum(x);

    public ReferenceEnumConverter()
         : base(convertToProviderExpression, convertFromProviderExpression)
    {
    }

    private static Guid ToGuid(Enum value)
    {
        ReferenceIdAttribute? attribute = value.GetType()
            ?.GetField(value.ToString())
            ?.GetCustomAttribute<ReferenceIdAttribute>();

        return attribute?.ReferenceId ?? throw new ArgumentException($"Can't convert value {(T)value} to Guid.");
    }

    private static T ToEnum(Guid referenceId)
    {
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);

        ReferenceIdAttribute? attribute;

        foreach (FieldInfo field in fields)
        {
            attribute = field.GetCustomAttribute<ReferenceIdAttribute>();

            if (attribute?.ReferenceId == referenceId)
                return Enum.Parse<T>(field.Name);
        }

        throw new ArgumentException($"Can't convert value {referenceId} to Enum.");
    }
}