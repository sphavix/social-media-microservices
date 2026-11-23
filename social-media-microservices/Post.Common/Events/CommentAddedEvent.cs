using cqrs.core.Events;

namespace Post.Common.Events;

public class CommentAddedEvent : BaseEvent
{
    public CommentAddedEvent() : base(nameof(CommentAddedEvent))
    {
    }

    public Guid CommentId { get; set; }
    public string Comments { get; set; }
    public string Username { get; set; }
    public DateTime DateAdded { get; set; }
}
