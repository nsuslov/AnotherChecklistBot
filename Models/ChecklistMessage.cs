namespace AnotherChecklistBot.Models;

public class ChecklistMessage
{
    public long Id { get; set; }
    public long ChatId { get; set; }
    public long MessageId { get; set; }

    public long ChecklistId { get; set; }
    public Checklist Checklist { get; set; } = default!;
}