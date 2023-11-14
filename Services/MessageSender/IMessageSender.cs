using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.MessageSender;

public interface IMessageSender
{
    public Task<Message> SendMessage(SendMessageRequest message);
    public void SendMessages(ICollection<SendMessageRequest> messages);
    public Task<Message> EditMessageText(EditMessageTextRequest message);
    public void EditMessageText(ICollection<EditMessageTextRequest> messages);
}