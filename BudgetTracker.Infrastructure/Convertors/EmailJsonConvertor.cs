using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using BudgetTracker.Domain.Models.User;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace BudgetTracker.Infrastructure.Convertors;

public class EmailJsonConvertor : Newtonsoft.Json.JsonConverter<Email>
{
    public override void WriteJson(JsonWriter writer, Email value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Value);
    }

    public override Email? ReadJson(JsonReader reader, Type objectType, Email existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var str = reader.Value.ToString();
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        return new Email(str);
    }
}