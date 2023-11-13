using AnotherChecklistBot.Models;

namespace AnotherChecklistBot.Data.Repositories;

public interface IChecklistMessageRepository {
    public ChecklistMessage? Get(long chatId, long checklistId);

    public ChecklistMessage AddOrUpdate(long chatId, long messageId, long checklistId);
    
}