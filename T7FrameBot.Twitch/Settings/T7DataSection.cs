namespace T7FrameBot.Twitch.Settings;

public sealed class T7DataSection
{
    public const string SectionName = "T7Data";

    public string JsonFilesPath { get; init; } = default!;
}