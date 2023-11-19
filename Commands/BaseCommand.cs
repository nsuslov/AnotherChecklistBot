using Telegram.Bot;
using Telegram.Bot.Requests;
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

    public abstract Task<SendMessageRequest?> Execute(List<string> args, Message message, CancellationToken cancellationToken);
}