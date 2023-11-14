using Telegram.Bot.Requests;
using AnotherChecklistBot.Models;

namespace AnotherChecklistBot.Services.MessageBuilder;

public interface IMessageBuilder
{
    public SendMessageRequest BuildSendMessageRequest(Checklist checklist, long chatId);
    public EditMessageTextRequest BuildEditMessageTextRequest(Checklist checklist, long chatId, int messageId);
}