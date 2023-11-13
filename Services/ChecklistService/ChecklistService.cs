
using System.Text.Json;
using AnotherChecklistBot.Data.Repositories;
using AnotherChecklistBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AnotherChecklistBot.Services.ChecklistService;

public class ChecklistService : IChecklistService
{
    private static readonly Dictionary<bool, string> CheckIcons = new Dictionary<bool, string> {
        { true, "✅" },
        { false, "❌" }
    };

    private readonly IChecklistRepository _checklistRepository;
    private readonly IListItemRepository _listItemRepository;
    private readonly IChecklistMessageRepository _checklistMessageRepository;
    private readonly ITelegramBotClient _botClient;

    public ChecklistService(
        IChecklistRepository checklistRepository,
        IListItemRepository listItemRepository,
        IChecklistMessageRepository checklistMessageRepository,
        ITelegramBotClient botClient)
    {
        _checklistRepository = checklistRepository;
        _listItemRepository = listItemRepository;
        _checklistMessageRepository = checklistMessageRepository;
        _botClient = botClient;
    }

    public async Task CreateChecklist(ICollection<string> items, long chatId, long messageId)
    {
        var checklist = _checklistRepository.Create(
            sourceChatId: chatId,
            sourceMessageId: messageId,
            items: items
        );
        var message = await SendMessage(checklist, chatId);
        _checklistMessageRepository.AddOrUpdate(chatId, message.MessageId, checklist.Id);
    }

    private async Task<Message> SendMessage(Checklist checklist, long chatId) {
        var itemsCount = checklist.ListItems.Count;
        var checkedCount = checklist.ListItems.Count(item => item.Checked);
        var title = $"Список: {checkedCount} из {itemsCount}";
        var sharedUrl = $"https://t.me/AnotherChecklistBot?start={checklist.Id}-{checklist.Secret}";
        var text = $"<b><a href=\"{sharedUrl}\">{title}</a></b>";
        var replyMarkup = new InlineKeyboardMarkup(
            checklist.ListItems.Select(item => {
                var callbackData = $"{checklist.Id}:{item.Id}:{!item.Checked}";
                return new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(
                    text: $"{CheckIcons[item.Checked]} {item.Text}{new string(' ', 100)}",
                    callbackData: callbackData
                )}; 
            })
        );
        return await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
            replyMarkup: replyMarkup
        );
    }
    
    public Task JoinChecklist(long checklistId, Guid secret, long chatId, long messageId)
    {
        throw new NotImplementedException();
    }
    
    public async Task CheckItem(long checklistId, long listItemId, bool check, long chatId)
    {
        var checklistMessage = _checklistMessageRepository.Get(chatId, checklistId);
        if (checklistMessage is null) return;

        var listItem = _listItemRepository.SetChecked(
            checklistId: checklistId,
            itemId: listItemId,
            value: check
        );
        var checklist = _checklistRepository.GetById(listItem.ChecklistId);
        await EditMessage(checklist, chatId, checklistMessage.MessageId);
    }

    private async Task<Message> EditMessage(Checklist checklist, long chatId, long messageId) {
        var items = checklist.ListItems.OrderBy(i => i.Id).ToList();
        var itemsCount = items.Count;
        var checkedCount = items.Count(item => item.Checked);
        var title = $"Список: {checkedCount} из {itemsCount}";
        var sharedUrl = $"https://t.me/AnotherChecklistBot?start={checklist.Id}-{checklist.Secret}";
        var text = $"<b><a href=\"{sharedUrl}\">{title}</a></b>";
        var replyMarkup = new InlineKeyboardMarkup(
            items.Select(item => {
                var callbackData = $"{checklist.Id}:{item.Id}:{!item.Checked}";
                return new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(
                    text: $"{CheckIcons[item.Checked]} {item.Text}{new string(' ', 100)}",
                    callbackData: callbackData
                )}; 
            })
        );

        return await _botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: (int)messageId,
            text: text,
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
            replyMarkup: replyMarkup
        );
    }
}