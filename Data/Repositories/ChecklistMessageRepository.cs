using AnotherChecklistBot.Data.Context;
using AnotherChecklistBot.Models;

namespace AnotherChecklistBot.Data.Repositories;

public class ChecklistMessageRepository : IChecklistMessageRepository
{
    private readonly ApplicationContext _context;

    public ChecklistMessageRepository(ApplicationContext context)
    {
        _context = context;
    }

    public ChecklistMessage AddOrUpdate(long chatId, long messageId, long checklistId)
    {
        var checklistMessage = Get(chatId, checklistId);
        if (checklistMessage is null) {
            checklistMessage = new ChecklistMessage
            {
                ChatId = chatId,
                ChecklistId = checklistId,
                MessageId = messageId
            };
            _context.ChecklistMessages.Add(checklistMessage);
        } else {
            checklistMessage.MessageId = messageId;
        }
        _context.SaveChanges();
        return checklistMessage;
    }

    public ChecklistMessage? Get(long chatId, long checklistId) => _context.ChecklistMessages
        .FirstOrDefault(e => (e.ChatId == chatId) && (e.ChecklistId == checklistId));
}