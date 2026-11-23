
using cqrs.core.Events;

namespace Post.Common.Events;

public class CommentUpdatedEvent : BaseEvent
{
    public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
    {
    }

    public Guid CommentId { get; set; }
    public string Comments { get; set; }
    public string Username { get; set; }
    public DateTime DateUpdated { get; set; }
}
