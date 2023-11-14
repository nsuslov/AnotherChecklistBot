using Telegram.Bot.Requests;
using AnotherChecklistBot.Models;

namespace AnotherChecklistBot.Services.MessageBuilder;

public interface IMessageBuilder
{
    public Task<SendMessageRequest> BuildSendMessageRequest(Checklist checklist, long chatId);
    public Task<EditMessageTextRequest> BuildEditMessageTextRequest(Checklist checklist, long chatId, int messageId);
}