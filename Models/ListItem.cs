namespace AnotherChecklistBot.Models;

public class ListItem
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool Checked { get; set; }

    public long ChecklistId { get; set; }
    public Checklist Checklist { get; set; } = default!;
}