using Telegram.Bot;
using Telegram.Bot.Types;
using AnotherChecklistBot.Models;
using AnotherChecklistBot.Services.ChecklistService;
using System.Text.RegularExpressions;
using AnotherChecklistBot.Services.CommandService;

namespace AnotherChecklistBot.Services.CommandHandler;

public class CommandHandler(
    ILogger<CommandHandler> logger,
    ICommandService commandService) : ICommandHandler
{
    private readonly ILogger<CommandHandler> _logger = logger;
    private readonly ICommandService _commandService = commandService;

    public async Task OnCommandAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if (message.Text is null) return;
        if (message.From is null) return;

        var commandLine = message.Text.Substring(1);
        var args = ParseCommand(commandLine);

        if (!args.Any()) return;
        await _commandService.TryExecuteCommand(args, message, cancellationToken);
    }

    private List<string> ParseCommand(string command)
    {
        List<string> parameters = new List<string>();

        var regex = new Regex(@"[^\s""']+|""([^""]*)""|'([^']*)'");
        var matches = regex.Matches(command);

        foreach (Match match in matches)
        {
            if (match.Groups[1].Success)
            {
                parameters.Add(match.Groups[1].Value);
            }
            else if (match.Groups[2].Success)
            {
                parameters.Add(match.Groups[2].Value);
            }
            else
            {
                parameters.Add(match.Value);
            }
        }

        return parameters;
    }

}