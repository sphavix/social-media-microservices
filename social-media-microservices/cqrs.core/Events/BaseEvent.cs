using cqrs.core.Messages;

namespace cqrs.core.Events;

public abstract class BaseEvent : Message
{
    protected BaseEvent(string type)
    {
        this.Type = type; // Set the Type property to the name of the derived event class
    }
    public int Version { get; set; }

    /// <summary>
    /// // We can use this property to store the type of the event, which can be useful for polymorphic data binding when we serialization, 
    /// and deserialization purposes. 
    /// It allows us to identify the specific event type when processing events in our system.
    /// </summary>
    public string Type { get; set; } 
}
