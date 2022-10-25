using T7FrameBot.Twitch.Collections.Entities;
using T7FrameBot.Twitch.SearchEngine;

namespace T7FrameBot.Twitch.Response;

public sealed class ResponseMessageBuilder : IResponseMessageBuilder
{
    public static IResponseMessageBuilder Default { get; } = new ResponseMessageBuilder();

    public string Build(BotChatCommand chatCommand, SearchResult searchResult)
    {
        return searchResult switch
        {
            { FighterNames.Count: 0 } => @$"Character ""{chatCommand.Name}"" does not exist",
            { FighterNames.Count: > 1 } => @$"Similar characters: {string.Join(", ", searchResult.FighterNames.Select(name => $@"""{name}"""))}",
            { Moves.Count: 0 } => @$"Move ""{chatCommand.Body}"" not found",
            { Moves.Count: > 1 } => $"Similar moves from \"{searchResult.FighterNames[0]}\": " +
                $"{string.Join(", ", searchResult.Moves.Select(m => $@"""{m.Command}"""))}",
            _ => $"🔸 {searchResult.FighterNames[0]} 🔸 {BuildMoveDescription(searchResult.Moves[0])}",
        };
    }

    private static string BuildMoveDescription(T7Move move)
    {
        return string.Join(" / ", new[]
            {
                NullIfEmpty("Command", move.Command),
                NullIfEmpty("HitLevel", move.HitLevel),
                NullIfEmpty("Damage", move.Damage),
                NullIfEmpty("StartUpFrame", move.StartUpFrame),
                NullIfEmpty("BlockFrame", move.BlockFrame),
                NullIfEmpty("HitFrame", move.HitFrame),
                NullIfEmpty("CounterHitFrame", move.CounterHitFrame),
                NullIfEmpty("Notes", move.Notes),
                NullIfEmpty("Gif", move.Gif),
            }
            .Where(part => !string.IsNullOrWhiteSpace(part)));
    }

    private static string? NullIfEmpty(string name, string? value)
    {
        return !string.IsNullOrWhiteSpace(value)
            ? $"{name}: {value}"
            : null;
    }
}