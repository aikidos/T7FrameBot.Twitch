namespace T7FrameBot.Twitch.Settings;

public sealed class TwitchSection
{
    public const string SectionName = "Twitch";

    public string UserName { get; init; } = default!;

    public string Token { get; init; } = default!;

    public IReadOnlyList<string> JoinChannels { get; init; } = default!;
}
