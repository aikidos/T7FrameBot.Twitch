namespace T7FrameBot.Twitch.Settings;

public sealed class CommandSection
{
    public IReadOnlyList<string> Prefixes { get; init; } = default!;
}