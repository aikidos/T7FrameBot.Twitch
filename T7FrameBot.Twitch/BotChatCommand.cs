using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace T7FrameBot.Twitch;

public sealed record BotChatCommand(string Prefix, string Name, string Body)
{
    private const string Pattern = @"^(?'name'\p{L}+)(\s+(?'body'.+)|)";

    public static bool TryParse(string message, IEnumerable<string> prefixes, [NotNullWhen(true)] out BotChatCommand? command)
    {
        command = null;

        var prefix = prefixes
            .FirstOrDefault(prefix => message.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase));

        if (prefix is null)
            return false;

        var input = message[prefix.Length..]
            .Trim();

        var match = Regex.Match(input, Pattern);

        if (!match.Success)
            return false;

        command = new BotChatCommand(
            prefix,
            match.Groups["name"].Value
                .ToLower()
                .TrimEnd(),
            match.Groups["body"].Value
                .Trim());

        return true;
    }
}