using System.Reflection;
using AnotherChecklistBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.CommandService;

public class CommandService : ICommandService
{
    private readonly Lazy<Dictionary<string, CommandEntity>> _commandEntities;
    private readonly object[] _commandConstructorArgs;

    public CommandService(
        IServiceScopeFactory scopeFactory,
        ILoggerFactory loggerFactory,
        ITelegramBotClient botClient)
    {
        _commandConstructorArgs = [scopeFactory, loggerFactory, botClient];
        _commandEntities = new(() => GetCommandsFromAssembly());
    }

    public async Task TryExecuteCommand(List<string> args, Message message, CancellationToken cancellationToken)
    {
        if (message.From is null) return;

        var commandName = args.First();
        var commandEntity = _commandEntities.Value.GetValueOrDefault(commandName);
        if (commandEntity is null) return;

        var newArgs = args[1..];

        await commandEntity.Instance.Execute(newArgs, message, cancellationToken);
    }

    private Dictionary<string, CommandEntity> GetCommandsFromAssembly()
    {
        var result = new Dictionary<string, CommandEntity>();

        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        var classesWithAttribute = types.Where(t => t.GetCustomAttribute<CommandAttribute>() != null).ToArray();

        foreach (var classType in classesWithAttribute)
        {
            var attribute = classType.GetCustomAttribute<CommandAttribute>();
            if (!typeof(BaseCommand).IsAssignableFrom(classType) || attribute is null) continue;
            var instance = Activator.CreateInstance(classType, _commandConstructorArgs) as BaseCommand;
            if (instance is null) throw new ArgumentException($"Unable to create an instance of {classType}");
            var name = attribute.Name.ToLower();
            result[name] = new()
            {
                Name = name,
                Description = attribute.Description,
                Instance = instance
            };
        }

        return result;
    }
}