using AnotherChecklistBot.Models;

namespace AnotherChecklistBot.Data.Repositories;

public interface IChecklistMessageRepository
{
    public ChecklistMessage? Get(long chatId, long checklistId);
    public ICollection<ChecklistMessage> GetAllByChecklistId(long checklistId);
    public ChecklistMessage AddOrUpdate(long chatId, int messageId, long checklistId);

}