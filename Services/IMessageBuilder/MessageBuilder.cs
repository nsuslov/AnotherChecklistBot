using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using AnotherChecklistBot.Models;

namespace AnotherChecklistBot.Services.MessageBuilder;

public class MessageBuilder : IMessageBuilder
{
    private static readonly Dictionary<bool, string> CheckIcons = new Dictionary<bool, string> {
        { true, "✅" },
        { false, "❌" }
    };
    private readonly Lazy<string> _botUsername;

    public MessageBuilder(ITelegramBotClient botClient)
    {
        _botUsername = new Lazy<string>(() => botClient.GetMeAsync().Result.Username ?? "");
    }

    public SendMessageRequest BuildSendMessageRequest(Checklist checklist, long chatId)
    {
        var text = GetMessageText(checklist);
        var replyMarkup = GetReplyMarkup(checklist);
        return new SendMessageRequest(chatId, text)
        {
            ReplyMarkup = replyMarkup,
            ParseMode = ParseMode.Html
        };
    }

    public EditMessageTextRequest BuildEditMessageTextRequest(Checklist checklist, long chatId, int messageId)
    {
        var text = GetMessageText(checklist);
        var replyMarkup = GetReplyMarkup(checklist);
        return new EditMessageTextRequest(chatId, messageId, text)
        {
            ReplyMarkup = replyMarkup,
            ParseMode = ParseMode.Html
        };
    }

    private string GetMessageText(Checklist checklist)
    {
        var itemsCount = checklist.ListItems.Count;
        var checkedCount = checklist.ListItems.Count(item => item.Checked);
        var title = $"Список: {checkedCount} из {itemsCount}";

        var sharedUrl = $"https://t.me/{_botUsername.Value}?start={checklist.Id}-{checklist.Secret}";

        return $"<a href=\"{sharedUrl}\"><b>{title}</b></a>";
    }

    private InlineKeyboardMarkup GetReplyMarkup(Checklist checklist)
    {
        var replyMarkup = new InlineKeyboardMarkup(
            checklist.ListItems
                .OrderBy(item => item.Id)
                .Select(item => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(
                    text: $"{CheckIcons[item.Checked]} {item.Text}{new string(' ', 100)}",
                    callbackData: $"{checklist.Id}:{item.Id}:{!item.Checked}"
                )})
        );
        return replyMarkup;
    }
}
