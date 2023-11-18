using AnotherChecklistBot.Data.Context;
using AnotherChecklistBot.Models;
using Microsoft.EntityFrameworkCore;

namespace AnotherChecklistBot.Data.Repositories;

public class ChecklistRepository : IChecklistRepository
{
    private readonly ApplicationContext _context;

    public ChecklistRepository(ApplicationContext context)
    {
        _context = context;
    }

    public Checklist Create(long sourceChatId, int sourceMessageId, ICollection<string> items)
    {
        var listItems = items.Select(item => new ListItem
        {
            Text = item
        }).ToList();
        var checklist = new Checklist
        {
            Secret = Guid.NewGuid(),
            SourceChatId = sourceChatId,
            SourceMessageId = sourceMessageId,
            ListItems = listItems
        };
        _context.Checklists.Add(checklist);
        _context.SaveChanges();
        return checklist;
    }

    public Checklist? GetById(long id) =>
        _context.Checklists.Include(e => e.ListItems).FirstOrDefault(e => e.Id == id);
}