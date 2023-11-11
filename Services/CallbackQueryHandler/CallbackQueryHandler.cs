using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.CallbackQueryHandler;

public class CallbackQueryHandler : ICallbackQueryHandler
{
    public Task OnCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}