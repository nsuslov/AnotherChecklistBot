using System.Collections.Concurrent;
using System.Timers;
using AnotherChecklistBot.Configurations;
using AnotherChecklistBot.Services.MessageSender;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Timer = System.Timers.Timer;

public class QueueMessageSender : IMessageSender
{
    private readonly ILogger<QueueMessageSender> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly CancellationToken _cancellationToken;
    private readonly ConcurrentQueue<IRequest<Message>> _messageQueue = new();
    private readonly Timer _timer;
    private bool _isProcessing = false;

    public QueueMessageSender(
        ILogger<QueueMessageSender> logger,
        ITelegramBotClient botClient,
        IConfiguration configuration,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _botClient = botClient;
        _cancellationToken = hostApplicationLifetime.ApplicationStopping;

        var telegramConfig = configuration.GetConfiguration<AnotherChecklistBot.Configurations.Telegram>();
        _timer = new()
        {
            Interval = telegramConfig.SendMessageDelayMs,
            AutoReset = false,

        };
        _timer.Elapsed += TimerElapsed;
    }

    public async Task<Message> SendMessage(SendMessageRequest message) =>
        await SendRequestAsync(message);

    public void SendMessages(ICollection<SendMessageRequest> messages)
    {
        foreach (var message in messages)
        {
            _messageQueue.Enqueue(message);
        }
        TryProcessingAsync();
    }

    public async Task<Message> EditMessageText(EditMessageTextRequest message) =>
        await SendRequestAsync(message);

    public void EditMessageText(ICollection<EditMessageTextRequest> messages)
    {
        foreach (var message in messages)
        {
            _messageQueue.Enqueue(message);
        }
        TryProcessingAsync();
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e) => ProcessQueueAsync();

    private void TryProcessingAsync()
    {
        if (!_isProcessing)
        {
            _isProcessing = true;
            ProcessQueueAsync();
        }
    }

    private void ProcessQueueAsync()
    {
        try
        {
            if (_messageQueue.TryPeek(out var message))
            {
                _ = SendRequestAsync(message);
                _messageQueue.TryDequeue(out _);
            }
            else
            {
                _isProcessing = false;
                _timer.Stop();
                return;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
        finally
        {
            _timer.Start();
        }
    }

    private async Task<Message> SendRequestAsync(IRequest<Message> request) =>
        await _botClient.MakeRequestAsync(request, _cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
}