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
        ReferenceByProductEnum enumAttr = value.GetType().GetCustomAttribute<ReferenceByProductEnum>()
            ?? throw new ArgumentException($"ReferenceByProductEnum is missing for enum {value.GetType().FullName}.");

        CodeAttribute? codeAttr = value.GetType()
            ?.GetField(value.ToString())
            ?.GetCustomAttribute<CodeAttribute>();

        string code = codeAttr?.Code ?? value.ToString();

        ReferenceByProductCommon? r = ReferenceByProductProviderStaticHost.ReferenceByProductCache.FirstOrDefault(x => x.Code == code
            && x.Entity == enumAttr.Entity
            && x.Reference == enumAttr.Reference
            && x.ProductShortName == enumAttr.ProductShortName);

        if (r == null)
            // TODO: better Exception
            throw new ArgumentException($"Can't convert value {(T)value} to Guid.");

        return r.Id;
    }

    private static T ToEnum(Guid referenceId)
    {
        ReferenceByProductCommon? r = ReferenceByProductProviderStaticHost.ReferenceByProductCache.FirstOrDefault(x => x.Id == referenceId);

        if (r == null)
            // TODO: better Exception
            throw new ArgumentException($"Can't convert value {referenceId} to Enum.");

        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);

        CodeAttribute? attribute;

        foreach (FieldInfo field in fields)
        {
            attribute = field.GetCustomAttribute<CodeAttribute>();

            if (attribute?.Code == r.Code || field.Name == r.Code)
                return Enum.Parse<T>(field.Name);
        }

        throw new ArgumentException($"Can't convert value {referenceId} to Enum.");
    }
}