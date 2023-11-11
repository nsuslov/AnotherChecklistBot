

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace AnotherChecklistBot.Services.ReceiverService;

public class ReceiverService : BackgroundService
{
    private readonly ILogger<ReceiverService> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateHandler _updateHandler;

    public ReceiverService(
        ILogger<ReceiverService> logger,
        ITelegramBotClient botClient,
        IUpdateHandler updateHandler)
    {
        _logger = logger;
        _botClient = botClient;
        _updateHandler = updateHandler;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartReceiving(stoppingToken);
        return Task.CompletedTask;
    }

    private void StartReceiving(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _botClient.ReceiveAsync(
            updateHandler: _updateHandler.HandleUpdateAsync,
            pollingErrorHandler: _updateHandler.HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken
        );
    }
}