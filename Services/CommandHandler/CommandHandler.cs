using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.CommandHandler;

public class CommandHandler : ICommandHandler
{
    public Task OnCommandAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}