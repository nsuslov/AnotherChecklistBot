using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.MessageSender;

public interface IMessageSender
{
    public void SendMessage(SendMessageRequest message);
    public void SendMessage(ICollection<SendMessageRequest> messages);
    public void EditMessageText(EditMessageTextRequest message);
    public void EditMessageText(ICollection<EditMessageTextRequest> messages);
}