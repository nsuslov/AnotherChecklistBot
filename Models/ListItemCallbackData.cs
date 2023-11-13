namespace AnotherChecklistBot.Models;

public class ListItemCallbackData {
    public long ChecklistId { get; set; }
    public long ListItemId { get; set; }
    public bool Check { get; set; }
}