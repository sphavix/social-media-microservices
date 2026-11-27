using cqrs.core.Events;

namespace cqrs.core.Domain;

public abstract class AggregateRoot
{
    protected Guid _id;
    private readonly List<BaseEvent> _changes = new(); // List to store the changes (events) that have occurred in the aggregate

    public Guid Id => _id; // Public property to access the aggregate's ID

    public int Version { get; set; } = -1; // Public property to access the aggregate's version

    public IEnumerable<BaseEvent> GetUncommittedChanges() => _changes; // Method to get the uncommitted changes (events) of the aggregate

    public void MarkChangesAsCommitted() => _changes.Clear(); // Method to mark the changes as committed (clear the list of changes)

    private void ApplyChange(BaseEvent @event, bool isNew = true) // Method to apply a change (event) to the aggregate
    {
        var method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() }); // Use reflection to find the appropriate Apply method for the event type
        if (method == null)
        {
            throw new InvalidOperationException($"The Apply method for event type {@event.GetType().Name} is not implemented in aggregate for {GetType().Name}.");
        }
        method.Invoke(this, new object[] { @event }); // Invoke the Apply method with the event as a parameter
        if (isNew)
        {
            _changes.Add(@event); // If the event is new, add it to the list of changes
        }
    }

    protected void RaiseEvent(BaseEvent @event) => ApplyChange(@event); // Method to raise an event (apply a change) to the aggregate

    public void ReplayEvents(IEnumerable<BaseEvent> events) // Method to replay events (apply changes) to the aggregate to recreate the state of the aggregate before the events occurred
    {
        foreach (var @event in events)
        {
            ApplyChange(@event, false); // Apply each event without marking it as new
            Version++; // Increment the version of the aggregate for each applied event
        }
    }
}
