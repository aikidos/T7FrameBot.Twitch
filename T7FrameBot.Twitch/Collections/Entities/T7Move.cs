using System.Text.Json.Serialization;
using T7FrameBot.Twitch.Collections.JsonConverters;

namespace T7FrameBot.Twitch.Collections.Entities;

public sealed class T7Move
{
    [JsonConverter(typeof(StringToArrayStringConverter))]
    public string[]? Alias { get; init; }

    [JsonPropertyName("Block frame")]
    [JsonConverter(typeof(IntToStringJsonConverter))]
    public string? BlockFrame { get; init; }

    public string? Command { get; init; }

    [JsonPropertyName("Counter hit frame")]
    public string? CounterHitFrame { get; init; }

    public string? Damage { get; init; }

    public string? Gif { get; init; }

    [JsonPropertyName("Hit frame")]
    public string? HitFrame { get; init; }

    [JsonPropertyName("Hit level")]
    public string? HitLevel { get; init; }

    public string? Notes { get; init; }

    [JsonPropertyName("Start up frame")]
    public string? StartUpFrame { get; init; }

    public string[]? Tags { get; init; }
}