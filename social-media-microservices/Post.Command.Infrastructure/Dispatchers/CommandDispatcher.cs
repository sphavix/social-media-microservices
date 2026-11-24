using cqrs.core.Commands;
using cqrs.core.Infrastructure;

namespace Post.Command.Infrastructure.Dispatchers;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new(); // Dictionary to store command handlers
    public void RegisterCommand<T>(Func<T, Task> handler) where T : BaseCommand
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException($"Handler for command type {typeof(T).Name} is not registered.");
        }
        _handlers.Add(typeof(T), (command) => handler((T)command)); // Cast the BaseCommand to the specific command type T
    }

    public async Task SendAsync(BaseCommand command)
    {
        if (_handlers.TryGetValue(command.GetType(), out Func<BaseCommand, Task>? handler)) // Check if a handler is registered for the command type
        {
            await handler(command); // Invoke the registered handler for the command
        }

        throw new ArgumentNullException(nameof(handler), "No handler registered for this command.");
    }
}
