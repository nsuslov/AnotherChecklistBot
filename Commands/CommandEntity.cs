namespace AnotherChecklistBot.Commands;

public class CommandEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public BaseCommand Instance { get; set; } = default!;
}