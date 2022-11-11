using T7FrameBot.Twitch.Collections;

namespace T7FrameBot.Twitch.SearchEngine;

public interface IT7JsonDataSearchEngine
{
    SearchResult Search(IT7JsonDataCollection jsonDataCollection, string fighterName, string commandName);
}