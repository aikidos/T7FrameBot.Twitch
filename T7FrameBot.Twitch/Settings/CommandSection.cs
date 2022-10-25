namespace T7FrameBot.Twitch.Settings;

public sealed class CommandSection
{
    public const string SectionName = "Command";

    public IReadOnlyList<string> Prefixes { get; init; } = default!;
}