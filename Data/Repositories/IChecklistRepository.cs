using AnotherChecklistBot.Models;

namespace AnotherChecklistBot.Data.Repositories;

public interface IChecklistRepository
{
    public Checklist Create(long sourceChatId, int sourceMessageId, ICollection<string> items);
    public Checklist? GetById(long id);
}