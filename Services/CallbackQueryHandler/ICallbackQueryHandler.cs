using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.CallbackQueryHandler;

public interface ICallbackQueryHandler
{
    public Task OnCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken);
}