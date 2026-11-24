
using cqrs.core.Commands;

namespace cqrs.core.Infrastructure;

public interface ICommandDispatcher
{
    void RegisterCommand<T>(Func<T, Task> handler) where T : BaseCommand;
    Task SendAsync(BaseCommand command);
}
