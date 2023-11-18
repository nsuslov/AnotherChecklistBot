
using Telegram.Bot;
using AnotherChecklistBot.Data.Repositories;
using AnotherChecklistBot.Services.MessageBuilder;
using AnotherChecklistBot.Services.MessageSender;

namespace AnotherChecklistBot.Services.ChecklistService;

public class ChecklistService : IChecklistService
{
    private readonly IChecklistRepository _checklistRepository;
    private readonly IListItemRepository _listItemRepository;
    private readonly IChecklistMessageRepository _checklistMessageRepository;
    private readonly ITelegramBotClient _botClient;
    private readonly IMessageBuilder _messageBuilder;
    private readonly IMessageSender _messageSender;

    public ChecklistService(
        IChecklistRepository checklistRepository,
        IListItemRepository listItemRepository,
        IChecklistMessageRepository checklistMessageRepository,
        ITelegramBotClient botClient,
        IMessageBuilder messageBuilder,
        IMessageSender messageSender)
    {
        _checklistRepository = checklistRepository;
        _listItemRepository = listItemRepository;
        _checklistMessageRepository = checklistMessageRepository;
        _botClient = botClient;
        _messageBuilder = messageBuilder;
        _messageSender = messageSender;
    }

    public async Task CreateChecklist(ICollection<string> items, long chatId, int messageId)
    {
        var checklist = _checklistRepository.Create(
            sourceChatId: chatId,
            sourceMessageId: messageId,
            items: items
        );

        var sendMessageRequest = _messageBuilder.BuildSendMessageRequest(checklist, chatId);
        var message = await _messageSender.SendMessage(sendMessageRequest);
        _checklistMessageRepository.AddOrUpdate(chatId, message.MessageId, checklist.Id);
    }

    public async Task JoinChecklist(long checklistId, Guid secret, long chatId, int messageId)
    {
        var checklist = _checklistRepository.GetById(checklistId);
        if (checklist is null) return;
        if (secret != checklist.Secret) return;

        var oldMessage = _checklistMessageRepository.Get(chatId, checklistId);
        if (oldMessage is not null)
        {
            await _botClient.DeleteMessageAsync(
                chatId: oldMessage.ChatId,
                messageId: oldMessage.MessageId
            );
        }

        var sendMessageRequest = _messageBuilder.BuildSendMessageRequest(checklist, chatId);
        var message = await _messageSender.SendMessage(sendMessageRequest);
        _checklistMessageRepository.AddOrUpdate(chatId, message.MessageId, checklist.Id);
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
        if (listItem is null) return;
        var checklist = _checklistRepository.GetById(listItem.ChecklistId);
        if (checklist is null) return;
        var editMessageTextRequest = _messageBuilder.BuildEditMessageTextRequest(checklist, chatId, checklistMessage.MessageId);
        await _messageSender.EditMessageText(editMessageTextRequest);
        var checklistMessages = _checklistMessageRepository.GetAllByChecklistId(checklistId);
        var editMessageTextRequests = checklistMessages
            .Where(e => e.ChatId != chatId)
            .Select(e => _messageBuilder.BuildEditMessageTextRequest(checklist, e.ChatId, e.MessageId))
            .ToList();
        _messageSender.EditMessageText(editMessageTextRequests);
    }

}