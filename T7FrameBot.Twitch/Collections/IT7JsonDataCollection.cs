using T7FrameBot.Twitch.Collections.Entities;

namespace T7FrameBot.Twitch.Collections;

public interface IT7JsonDataCollection
{
    IEnumerable<string> GetAllFighterNames();

    IEnumerable<T7Move> GetMovesByFighterName(string fighterName);
}