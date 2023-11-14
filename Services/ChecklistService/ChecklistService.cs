
using AnotherChecklistBot.Data.Repositories;
using AnotherChecklistBot.Services.MessageBuilder;
using AnotherChecklistBot.Services.MessageSender;

namespace AnotherChecklistBot.Services.ChecklistService;

public class ChecklistService : IChecklistService
{
    private readonly IChecklistRepository _checklistRepository;
    private readonly IListItemRepository _listItemRepository;
    private readonly IChecklistMessageRepository _checklistMessageRepository;
    private readonly IMessageBuilder _messageBuilder;
    private readonly IMessageSender _messageSender;

    public ChecklistService(
        IChecklistRepository checklistRepository,
        IListItemRepository listItemRepository,
        IChecklistMessageRepository checklistMessageRepository,
        IMessageBuilder messageBuilder,
        IMessageSender messageSender)
    {
        _checklistRepository = checklistRepository;
        _listItemRepository = listItemRepository;
        _checklistMessageRepository = checklistMessageRepository;
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

        var sendMessageRequest = await _messageBuilder.BuildSendMessageRequest(checklist, chatId);
        var message = await _messageSender.SendMessage(sendMessageRequest);
        _checklistMessageRepository.AddOrUpdate(chatId, message.MessageId, checklist.Id);
    }

    public Task JoinChecklist(long checklistId, Guid secret, long chatId, int messageId)
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
        if (listItem is null) return;
        var checklist = _checklistRepository.GetById(listItem.ChecklistId);
        if (checklist is null) return;
        var editMessageTextRequest = await _messageBuilder.BuildEditMessageTextRequest(checklist, chatId, checklistMessage.MessageId);
        await _messageSender.EditMessageText(editMessageTextRequest);
    }

}