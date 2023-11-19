using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Commands;

public abstract class BaseCommand(
    IServiceScopeFactory scopeFactory,
    ILoggerFactory loggerFactory,
    ITelegramBotClient botClient)
{
    protected IServiceScopeFactory ScopeFactory = scopeFactory;
    protected ILoggerFactory LoggerFactory = loggerFactory;
    protected ITelegramBotClient BotClient = botClient;

    public abstract Task Execute(List<string> args, Message message, CancellationToken cancellationToken);
}