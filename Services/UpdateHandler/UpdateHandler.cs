using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using AnotherChecklistBot.Services.MessageHandler;
using AnotherChecklistBot.Services.CommandHandler;
using AnotherChecklistBot.Services.CallbackQueryHandler;

namespace AnotherChecklistBot.Services.UpdateHandler;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public UpdateHandler(
        ILogger<UpdateHandler> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);
        await Task.Delay(5000, cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                if (update.Message is null) break;
                await HandleMessageAsync(botClient, update.Message, cancellationToken);
                break;
            case UpdateType.EditedMessage:
                if (update.EditedMessage is null) break;
                await HandleMessageAsync(botClient, update.EditedMessage, cancellationToken);
                break;
            case UpdateType.CallbackQuery:
                if (update.CallbackQuery is null) break;
                await HandleCallbackQueryAsync(botClient, update.CallbackQuery, cancellationToken);
                break;
            default:
                throw new NotImplementedException(update.Type.ToString());
        }
    }

    private async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(message.Text)) return;

        using var scope = _scopeFactory.CreateScope();

        if (message.Text.StartsWith("/"))
        {
            var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler>();
            await commandHandler.OnCommandAsync(botClient, message, cancellationToken);
        }
        else
        {
            var messageHandler = scope.ServiceProvider.GetRequiredService<IMessageHandler>();
            await messageHandler.OnMessageAsync(botClient, message, cancellationToken);
        }
    }

    private async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(callbackQuery.Data)) return;
        using var scope = _scopeFactory.CreateScope();
        var callbackQueryHandler = scope.ServiceProvider.GetRequiredService<ICallbackQueryHandler>();
        await callbackQueryHandler.OnCallbackQueryAsync(botClient, callbackQuery, cancellationToken);
    }
}