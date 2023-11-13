namespace AnotherChecklistBot.Models;

public class Checklist
{
    public long Id { get; set; }
    public Guid Secret { get; set; }
    public long SourceChatId { get; set; }
    public long SourceMessageId { get; set; }

    public ICollection<ListItem> ListItems { get; set; } = default!;
    public ICollection<ChecklistMessage> ChecklistMessages { get; set; } = default!;
}