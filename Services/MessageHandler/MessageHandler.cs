using AnotherChecklistBot.Services.MessageSender;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.MessageHandler;

public class MessageHandler : IMessageHandler
{
    private static readonly string[] Separators = { "\n", ",", ";", " " };
    private IMessageSender _messageSender;

    public MessageHandler(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }
    public Task OnMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if (message.From is null) return Task.CompletedTask;
        if (message.Text is null) return Task.CompletedTask;

        var items = SplitItems(message.Text);
        var messages = items.Select(item => new SendMessageRequest(message.From.Id, item)).ToArray();
        _messageSender.SendMessage(messages);
        return Task.CompletedTask;
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