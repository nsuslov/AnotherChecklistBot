using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.MessageHandler;

public interface IMessageHandler
{
    public Task OnMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
}