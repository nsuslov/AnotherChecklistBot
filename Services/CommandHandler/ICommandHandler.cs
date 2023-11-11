using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.CommandHandler;

public interface ICommandHandler
{
    public Task OnCommandAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
}