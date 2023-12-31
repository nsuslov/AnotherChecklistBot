using AnotherChecklistBot.Models;
using AnotherChecklistBot.Services.ChecklistService;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Commands;

[Command(name: "start", description: "Показывает стартовое сообщение.")]
public class StartCommand : BaseCommand
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StartCommand> _logger;

    public StartCommand(
        IServiceScopeFactory scopeFactory,
        ILoggerFactory loggerFactory,
        ITelegramBotClient botClient) : base(scopeFactory, loggerFactory, botClient)
    {
        _scopeFactory = scopeFactory;
        _logger = loggerFactory.CreateLogger<StartCommand>();
    }

    public override async Task<SendMessageRequest?> Execute(List<string> args, Message message, CancellationToken cancellationToken)
    {
        if (message.From is null) return null;

        if (!args.Any())
        {
            return await GetWelcomeMessage(message.From.Id);
        }
        else
        {
            var sharedData = DeserializeSharedLinkData(args[0]);
            using var scope = _scopeFactory.CreateScope();
            var checklistService = scope.ServiceProvider.GetRequiredService<IChecklistService>();
            await checklistService.JoinChecklist(
                checklistId: sharedData.ChecklistId,
                secret: sharedData.Secret,
                chatId: message.From.Id,
                messageId: message.MessageId
            );
            return null;
        }
    }

    private Task<SendMessageRequest> GetWelcomeMessage(long chatId) =>
        Task.FromResult(new SendMessageRequest(
            chatId: chatId,
            text: "Привет!"
        ));

    private SharedLinkData DeserializeSharedLinkData(string parameter)
    {
        try
        {
            var data = parameter.Split("-", 2);

            return new SharedLinkData
            {
                ChecklistId = long.Parse(data[0]),
                Secret = new Guid(data[1])
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}