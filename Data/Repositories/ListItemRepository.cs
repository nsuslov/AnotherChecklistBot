using AnotherChecklistBot.Data.Context;
using AnotherChecklistBot.Models;

namespace AnotherChecklistBot.Data.Repositories;

public class ListItemRepository : IListItemRepository
{
    private readonly ApplicationContext _context;

    public ListItemRepository(ApplicationContext context)
    {
        _context = context;
    }

    public ListItem? SetChecked(long checklistId, long itemId, bool value)
    {
        var listItem = _context.ListItems
            .FirstOrDefault(e => (e.Id == itemId) && (e.ChecklistId == checklistId));
        if (listItem is not null) {
            listItem.Checked = value;
            _context.SaveChanges();
        }
        return listItem;
    }
}