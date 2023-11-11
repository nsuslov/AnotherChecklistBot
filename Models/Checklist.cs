namespace AnotherChecklistBot.Models;

public class Checklist
{
    public long Id { get; set; }
    public Guid Secret { get; set; }
    public long SourceChatId { get; set; }
    public long SourceMessageId { get; set; }

    public List<ListItem> ListItems = default!;
    public List<ChecklistMessage> ChecklistMessages { get; set; } = default!;
}