using System.Text.Json;
using T7FrameBot.Twitch.Collections.Entities;

namespace T7FrameBot.Twitch.Collections;

public sealed class T7JsonDataCollection : IT7JsonDataCollection
{
    private const string GenericMovesFileName = "!generic";
    private const string SearchPattern = "*.json";

    private readonly IReadOnlyCollection<T7Move> _genericMoves;
    private readonly IReadOnlyDictionary<string, IReadOnlyList<T7Move>> _movesByFighterName;

    public static async Task<T7JsonDataCollection> LoadFromPathAsync(string path)
    {
        T7Move[]? genericMoves = null;
        var movesByFighterName = new Dictionary<string, IReadOnlyList<T7Move>>();

        foreach (var fileName in Directory.EnumerateFiles(path, SearchPattern))
        {
            var fighterName = Path.GetFileNameWithoutExtension(fileName)
                .ToLower()
                .Trim();

            await using var fileStream = File.OpenRead(fileName);

            var moves = await JsonSerializer.DeserializeAsync<T7Move[]>(fileStream);

            switch (fighterName)
            {
                case GenericMovesFileName:
                    genericMoves ??= moves;
                    break;

                default:
                    movesByFighterName.Add(fighterName, moves!);
                    break;
            }
        }

        return new T7JsonDataCollection(genericMoves ?? Array.Empty<T7Move>(), movesByFighterName);
    }

    private T7JsonDataCollection(IReadOnlyCollection<T7Move> genericMoves, IReadOnlyDictionary<string, IReadOnlyList<T7Move>> movesByFighterName)
    {
        _genericMoves = genericMoves;
        _movesByFighterName = movesByFighterName;
    }

    public IEnumerable<string> GetAllFighterNames()
    {
        return _movesByFighterName.Keys;
    }

    public IEnumerable<T7Move> GetMovesByFighterName(string fighterName)
    {
        fighterName = fighterName
            .ToLower()
            .Trim();

        return _movesByFighterName.GetValueOrDefault(fighterName)?
            .Concat(_genericMoves)
            .GroupBy(m => m.Command)
            .Select(g => g.First()) ?? Enumerable.Empty<T7Move>();
    }
}