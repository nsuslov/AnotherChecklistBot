using AnotherChecklistBot.Models;

namespace AnotherChecklistBot.Data.Repositories;

public interface IListItemRepository
{
    public ListItem? SetChecked(long checklistId, long itemId, bool value);
}