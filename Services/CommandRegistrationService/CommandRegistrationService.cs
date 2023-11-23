
using AnotherChecklistBot.Services.CommandService;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.CommandRegistrationService;

public class CommandRegistrationService(
    ICommandService commandService,
    ITelegramBotClient botClient) : BackgroundService
{
    private readonly ICommandService _commandService = commandService;
    private readonly ITelegramBotClient _botClient = botClient;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var commandEntities = _commandService.GetCommandEntities();
        var botCommands = commandEntities.Select(ce => new BotCommand
        {
            Command = ce.Name,
            Description = ce.Description
        }).ToList();
        await _botClient.SetMyCommandsAsync(botCommands, new BotCommandScopeDefault());
    }
}