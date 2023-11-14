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
    private readonly Lazy<Task<User>> _botUser;

    public MessageBuilder(ITelegramBotClient botClient)
    {
        _botUser = new Lazy<Task<User>>(() => botClient.GetMeAsync());
    }

    public async Task<SendMessageRequest> BuildSendMessageRequest(Checklist checklist, long chatId)
    {
        var text = await GetMessageText(checklist);
        var replyMarkup = GetReplyMarkup(checklist);
        return new SendMessageRequest(chatId, text)
        {
            ReplyMarkup = replyMarkup,
            ParseMode = ParseMode.Html
        };
    }

    public async Task<EditMessageTextRequest> BuildEditMessageTextRequest(Checklist checklist, long chatId, int messageId)
    {
        var text = await GetMessageText(checklist);
        var replyMarkup = GetReplyMarkup(checklist);
        return new EditMessageTextRequest(chatId, messageId, text)
        {
            ReplyMarkup = replyMarkup,
            ParseMode = ParseMode.Html
        };
    }

    private async Task<string> GetMessageText(Checklist checklist)
    {
        var itemsCount = checklist.ListItems.Count;
        var checkedCount = checklist.ListItems.Count(item => item.Checked);
        var title = $"Список: {checkedCount} из {itemsCount}";

        var username = (await _botUser.Value).Username;
        var sharedUrl = $"https://t.me/{username}?start={checklist.Id}-{checklist.Secret}";

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
