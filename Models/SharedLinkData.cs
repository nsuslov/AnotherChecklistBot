namespace AnotherChecklistBot.Models;

public class SharedLinkData
{
    public long ChecklistId { get; set; }
    public Guid Secret { get; set; }
}