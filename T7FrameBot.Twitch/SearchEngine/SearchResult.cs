using T7FrameBot.Twitch.Collections.Entities;

namespace T7FrameBot.Twitch.SearchEngine;

public sealed class SearchResult
{
    public static SearchResult Empty { get; } = new()
    {
        FighterNames = Array.Empty<string>(),
        Moves = Array.Empty<T7Move>(),
    };

    public IReadOnlyList<string> FighterNames { get; private init; } = default!;

    public IReadOnlyList<T7Move> Moves { get; private init; } = default!;

    private SearchResult()
    {
    }

    public SearchResult(IEnumerable<string> fighterNames, IEnumerable<T7Move>? moves = null)
    {
        FighterNames = fighterNames
            .ToArray();

        Moves = moves?.ToArray() ?? Array.Empty<T7Move>();
    }
}