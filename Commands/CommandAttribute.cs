namespace AnotherChecklistBot.Commands;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class CommandAttribute(
    string name,
    string description) : Attribute
{
    public string Name => name;
    public string Description = description;
}