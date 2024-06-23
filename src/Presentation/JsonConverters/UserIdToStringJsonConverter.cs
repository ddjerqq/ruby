
using Ruby.Generated;

namespace Presentation.JsonConverters;

public sealed class UserIdToStringJsonConverter : global::System.Text.Json.Serialization.JsonConverter<UserId>
{
    public override UserId Read(ref global::System.Text.Json.Utf8JsonReader reader, Type typeToConvert, global::System.Text.Json.JsonSerializerOptions options)
    {
        return UserId.Parse(reader.GetString());
    }

    public override void Write(global::System.Text.Json.Utf8JsonWriter writer, UserId value, global::System.Text.Json.JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}