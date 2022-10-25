namespace T7FrameBot.Twitch.SearchEngine;

public interface IT7JsonDataSearchEngine
{
    SearchResult Search(string fighterName, string commandName);
}