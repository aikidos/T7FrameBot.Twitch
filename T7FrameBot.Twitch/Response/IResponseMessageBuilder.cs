using T7FrameBot.Twitch.SearchEngine;

namespace T7FrameBot.Twitch.Response;

public interface IResponseMessageBuilder
{
    string Build(BotChatCommand chatCommand, SearchResult searchResult);
}