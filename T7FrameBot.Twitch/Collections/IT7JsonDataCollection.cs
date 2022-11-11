using T7FrameBot.Twitch.Collections.Entities;

namespace T7FrameBot.Twitch.Collections;

public interface IT7JsonDataCollection
{
    IReadOnlyList<string> AllFighterNames { get; }

    IEnumerable<T7Move> GetMovesByFighterName(string fighterName);
}