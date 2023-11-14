namespace AnotherChecklistBot.Services.ChecklistService;

public interface IChecklistService
{
    public Task CreateChecklist(ICollection<string> items, long chatId, int messageId);
    public Task JoinChecklist(long checklistId, Guid secret, long chatId, int messageId);
    public Task CheckItem(long checklistId, long listItemId, bool check, long chatId);
}