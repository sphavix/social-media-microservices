

using cqrs.core.Events;

namespace Post.Common.Events;

public class PostUpdatedEvent : BaseEvent
{
    public PostUpdatedEvent() : base(nameof(PostUpdatedEvent))
    {
    }
    public string Message { get; set; }
}
