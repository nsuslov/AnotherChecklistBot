using AnotherChecklistBot.Commands;
using Telegram.Bot.Types;

namespace AnotherChecklistBot.Services.CommandService;

public interface ICommandService
{
    public Task TryExecuteCommand(List<string> args, Message message, CancellationToken cancellationToken);
}