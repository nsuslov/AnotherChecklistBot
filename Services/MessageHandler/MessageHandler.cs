using AnotherChecklistBot.Services.MessageSender;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.MessageHandler;

public class MessageHandler : IMessageHandler
{
    private IMessageSender _messageSender;

    public MessageHandler(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }
    public Task OnMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if (message.From is null) return Task.CompletedTask;
        if (message.Text is null) return Task.CompletedTask;

        _messageSender.SendMessage(new SendMessageRequest(message.From.Id, message.Text));
        return Task.CompletedTask;
    }
}