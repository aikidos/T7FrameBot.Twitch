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

    var twitchSection = configuration
        .GetRequiredSection(TwitchSection.SectionName)
        .Get<TwitchSection>();

    var dataSection = configuration
        .GetRequiredSection(T7DataSection.SectionName)
        .Get<T7DataSection>();

    var commandSection = configuration
        .GetRequiredSection(CommandSection.SectionName)
        .Get<CommandSection>();

    var jsonDataCollection = await T7JsonDataCollection.LoadFromPathAsync(dataSection.JsonFilesPath);
    var searchEngine = new T7JsonDataSearchEngine(jsonDataCollection);

    var twitchClient = new TwitchClient(protocol: ClientProtocol.TCP);

    twitchClient.OnLog += (_, logArgs) => logger.Information("{Data}", logArgs.Data);
    twitchClient.OnMessageReceived += (_, receivedArgs) =>
    {
        var chatMessage = receivedArgs.ChatMessage;

        if (!BotChatCommand.TryParse(chatMessage.Message, commandSection.Prefixes, out var command))
            return;

        var searchResult = searchEngine.Search(command.Name, command.Body);

        var responseMessage = ResponseMessageBuilder.Default
            .Build(command, searchResult);

        twitchClient.SendMessage(chatMessage.Channel, responseMessage);
    };

    twitchClient.Initialize(new ConnectionCredentials(twitchSection.UserName, twitchSection.Token));

    twitchClient.Connect();

    foreach (var channelName in twitchSection.JoinChannels)
    {
        twitchClient.JoinChannel(channelName);
    }
}
catch (Exception ex)
{
    logger.Error(ex, "{Message}", ex.Message);
}

Console.ReadLine();