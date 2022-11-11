using Microsoft.Extensions.Configuration;
using Serilog;
using T7FrameBot.Twitch;
using T7FrameBot.Twitch.Collections;
using T7FrameBot.Twitch.Response;
using T7FrameBot.Twitch.SearchEngine;
using T7FrameBot.Twitch.Settings;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Models;

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("settings.json")
        .Build();

    var settings = configuration.Get<BotSettings>()!;

    var jsonDataCollection = await T7JsonDataCollection.LoadFromPathAsync(settings.T7Data.JsonFilesPath);
    var searchEngine = new T7JsonDataSearchEngine();
    var responseMessageBuilder = ResponseMessageBuilder.Default;

    var twitchClient = new TwitchClient(protocol: ClientProtocol.TCP);

    twitchClient.OnLog += (_, logArgs) => logger.Information("{Data}", logArgs.Data);
    twitchClient.OnMessageReceived += (_, receivedArgs) =>
    {
        var chatMessage = receivedArgs.ChatMessage;

        if (!BotChatCommand.TryParse(chatMessage.Message, settings.Command.Prefixes, out var command))
            return;

        var searchResult = searchEngine.Search(jsonDataCollection, command.Name, command.Body);

        var responseMessage = responseMessageBuilder.Build(command, searchResult);

        twitchClient.SendMessage(chatMessage.Channel, responseMessage);
    };

    twitchClient.Initialize(new ConnectionCredentials(settings.Twitch.UserName, settings.Twitch.Token));

    twitchClient.Connect();

    foreach (var channelName in settings.Twitch.JoinChannels)
    {
        twitchClient.JoinChannel(channelName);
    }
}
catch (Exception ex)
{
    logger.Error(ex, "{Message}", ex.Message);
}

Console.ReadLine();