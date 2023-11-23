using System.Collections.Immutable;
using Telegram.Bot.Types;
using AnotherChecklistBot.Commands;

namespace AnotherChecklistBot.Services.CommandService;

public interface ICommandService
{
    public Task TryExecuteCommand(List<string> args, Message message, CancellationToken cancellationToken);
    public ImmutableList<CommandEntity> GetCommandEntities();
}