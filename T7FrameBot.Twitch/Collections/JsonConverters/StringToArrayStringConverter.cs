using System.Text.Json;
using System.Text.Json.Serialization;

namespace T7FrameBot.Twitch.Collections.JsonConverters;

public sealed class StringToArrayStringConverter : JsonConverter<string[]>
{
    public override string[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new[] { reader.GetString()! },
            _ => JsonSerializer.Deserialize<string[]>(ref reader, options),
        };
    }

    public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}