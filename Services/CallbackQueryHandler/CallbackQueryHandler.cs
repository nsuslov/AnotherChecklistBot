using Telegram.Bot;
using Telegram.Bot.Types;
using AnotherChecklistBot.Models;
using AnotherChecklistBot.Services.ChecklistService;

namespace AnotherChecklistBot.Services.CallbackQueryHandler;

public class CallbackQueryHandler : ICallbackQueryHandler
{
    private readonly ILogger<CallbackQueryHandler> _logger;
    private readonly IChecklistService _checklistService;

    public CallbackQueryHandler(
        ILogger<CallbackQueryHandler> logger,
        IChecklistService checklistService)
    {
        _logger = logger;
        _checklistService = checklistService;
    }

    public async Task OnCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        System.Console.WriteLine(callbackQuery.Data);
        if (callbackQuery.Data is null) return;


        var callbackData = DeserializeCallbackData(callbackQuery.Data);
        await _checklistService.CheckItem(
            checklistId: callbackData.ChecklistId,
            listItemId: callbackData.ListItemId,
            check: callbackData.Check,
            chatId: callbackQuery.From.Id
        );
    }

    private ListItemCallbackData DeserializeCallbackData(string text) {
        try {
            var data = text.Split(":");
            return new ListItemCallbackData
            {
                ChecklistId = long.Parse(data[0]),
                ListItemId = long.Parse(data[1]),
                Check = bool.Parse(data[2])
            };
        } catch (Exception ex) {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}