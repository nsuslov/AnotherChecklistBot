using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.MessageHandler;

public class MessageHandler : IMessageHandler
{
    public Task OnMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}