using AnotherChecklistBot.Services.ChecklistService;
using AnotherChecklistBot.Services.MessageSender;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.MessageHandler;

public class MessageHandler : IMessageHandler
{
    private static readonly string[] Separators = { "\n", ",", ";", " " };
    private IMessageSender _messageSender;
    private IChecklistService _checklistService;

    public MessageHandler(
        IMessageSender messageSender,
        IChecklistService checklistService)
    {
        _messageSender = messageSender;
        _checklistService = checklistService;
    }
    public async Task OnMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if (message.From is null) return;
        if (message.Text is null) return;
        var items = SplitItems(message.Text);
        await _checklistService.CreateChecklist(items, message.From.Id, message.MessageId);
    }

    private string[] SplitItems(string text)
    {
        foreach (var separator in Separators)
        {
            var items = text.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (items.Length > 1) return items;
        }
        return new[] { text };
    }
}