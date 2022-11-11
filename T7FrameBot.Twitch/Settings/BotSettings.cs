namespace T7FrameBot.Twitch.Settings;

public sealed class BotSettings
{
    public TwitchSection Twitch { get; init; } = default!;

    public T7DataSection T7Data { get; init; } = default!;

    public CommandSection Command { get; init; } = default!;
}