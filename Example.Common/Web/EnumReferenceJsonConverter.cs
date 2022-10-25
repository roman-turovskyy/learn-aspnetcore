using Example.Domain.Enums;
using Newtonsoft.Json;
using System.Reflection;

namespace Example.Common.Web;

public class EnumReferenceJsonConverter : JsonConverter
{
    private static bool IsNullableType(Type t)
    {
        if (t is null)
            throw new ArgumentNullException(nameof(t));

        return (t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
    }

    public override bool CanConvert(Type objectType)
    {
        Type? t = IsNullableType(objectType)
            ? Nullable.GetUnderlyingType(objectType)
            : objectType;

        return t is not null && t.IsAssignableTo(typeof(EnumReference));
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        Type? t = objectType;

        try
        {
            if (reader.TokenType == JsonToken.String)
            {
                string? enumText = reader.Value?.ToString();

                if (string.IsNullOrEmpty(enumText))
                    return null;

                return Activator.CreateInstance(t, enumText);
            }
        }
        catch (Exception)
        {
            throw;
            //throw new JsonSerializationException($"Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(reader.Value), objectType), ex);
        }

        // we don't actually expect to get here.
        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing enum.");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        EnumReference e = (EnumReference)value;

        writer.WriteValue(e.ToString());
    }
}
