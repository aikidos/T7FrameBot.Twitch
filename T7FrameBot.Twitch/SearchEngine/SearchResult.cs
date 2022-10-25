using T7FrameBot.Twitch.Collections.Entities;

namespace T7FrameBot.Twitch.SearchEngine;

public sealed class SearchResult
{
    public IReadOnlyList<string> FighterNames { get; private init; } = default!;

    public IReadOnlyList<T7Move> Moves { get; private init; } = default!;

    public static SearchResult Empty { get; } = new()
    {
        FighterNames = Array.Empty<string>(),
        Moves = Array.Empty<T7Move>(),
    };

    public static SearchResult Create(IEnumerable<string> fighterNames, IEnumerable<T7Move>? moves = null)
    {
        return new SearchResult
        {
            FighterNames = fighterNames
                .ToArray(),
            Moves = moves?.ToArray() ?? Array.Empty<T7Move>(),
        };
    }
}