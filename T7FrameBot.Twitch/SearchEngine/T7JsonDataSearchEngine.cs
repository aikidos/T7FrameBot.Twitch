using F23.StringSimilarity;
using T7FrameBot.Twitch.Collections;
using T7FrameBot.Twitch.Collections.Entities;

namespace T7FrameBot.Twitch.SearchEngine;

public sealed class T7JsonDataSearchEngine : IT7JsonDataSearchEngine
{
    private const double SimilarityDistance = 0.5d;

    private readonly Dictionary<string, string> _aliasByFighterName = new()
    {
        { "dj", "devil_jin" },
        { "kaz", "kazuya" },
        { "ak", "armor_king" },
    };

    private readonly NormalizedLevenshtein _levenshtein = new();
    private readonly IT7JsonDataCollection _jsonDataCollection;

    public T7JsonDataSearchEngine(IT7JsonDataCollection jsonDataCollection)
    {
        _jsonDataCollection = jsonDataCollection;
    }

    public SearchResult Search(string fighterName, string commandName)
    {
        var fighterNames = SearchFighterNames(fighterName)
            .ToArray();

        return fighterNames switch
        {
            { Length: 0 } => SearchResult.Empty,
            { Length: > 1 } => SearchResult.Create(fighterNames),
            _ => SearchResult.Create(fighterNames, SearchMoves(fighterNames[0], commandName)),
        };
    }

    private IEnumerable<string> SearchFighterNames(string fighterName)
    {
        fighterName = fighterName
            .ToLower()
            .Trim();

        fighterName = _aliasByFighterName.GetValueOrDefault(fighterName) ?? fighterName;

        var fighterNames = _jsonDataCollection.GetAllFighterNames()
            .ToArray();

        var nameStartingWith = fighterNames
            .Where(name => name.StartsWith(fighterName))
            .Take(2)
            .ToArray();

        if (nameStartingWith is { Length: 1 })
            return new[] { nameStartingWith[0] };

        var foundNames = new List<string>();

        foreach (var name in fighterNames)
        {
            switch (fighterName)
            {
                case var _ when fighterName.Equals(name, StringComparison.InvariantCultureIgnoreCase):
                    return new[] { fighterName };

                case var _ when _levenshtein.Similarity(fighterName, name) >= SimilarityDistance:
                    foundNames.Add(name);
                    break;
            }
        }

        return foundNames;
    }

    private IEnumerable<T7Move> SearchMoves(string fighterName, string commandName)
    {
        commandName = SimplifyCommandName(commandName);

        var foundMoves = new List<T7Move>();

        foreach (var move in _jsonDataCollection.GetMovesByFighterName(fighterName)
            .Where(move => !string.IsNullOrWhiteSpace(move.Command)))
        {
            switch (SimplifyCommandName(move.Command!))
            {
                case var moveCommand when moveCommand.Equals(commandName, StringComparison.InvariantCultureIgnoreCase):
                    return new[] { move };

                case var moveCommand when _levenshtein.Similarity(moveCommand, commandName) >= SimilarityDistance:
                    foundMoves.Add(move);
                    break;
            }
        }

        return foundMoves;
    }

    private static string SimplifyCommandName(string commandName)
    {
        return commandName
            .Replace("/", string.Empty)
            .Replace("+", string.Empty)
            .Replace(",", string.Empty)
            .Replace("fff", "wr")
            .Replace("fnddf", "cd")
            .ToLower();
    }
}