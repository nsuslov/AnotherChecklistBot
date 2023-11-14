using Telegram.Bot;
using Telegram.Bot.Types;
using AnotherChecklistBot.Models;
using AnotherChecklistBot.Services.ChecklistService;

namespace AnotherChecklistBot.Services.CommandHandler;

public class CommandHandler : ICommandHandler
{
    private readonly ILogger<CommandHandler> _logger;
    private readonly IChecklistService _checklistService;

    public CommandHandler(
        ILogger<CommandHandler> logger,
        IChecklistService checklistService)
    {
        _logger = logger;
        _checklistService = checklistService;
    }

    public async Task OnCommandAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if (message.Text is null) return;
        if (message.From is null) return;

        var parameters = message.Text.Split(" ");
        if (parameters.Length < 2) return;

        var data = DeserializeSharedLinkData(parameters[1]);
        await _checklistService.JoinChecklist(
            checklistId: data.ChecklistId,
            secret: data.Secret,
            chatId: message.From.Id,
            messageId: message.MessageId
        );
    }

    private SharedLinkData DeserializeSharedLinkData(string parameter)
    {
        try
        {
            var data = parameter.Split("-", 2);

            return new SharedLinkData
            {
                ChecklistId = long.Parse(data[0]),
                Secret = new Guid(data[1])
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}