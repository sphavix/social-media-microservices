using cqrs.core.Domain;
using Post.Common.Events;
using System.Reflection.Metadata;

namespace Post.Command.Domain.Aggregates;

public class PostAggregate : AggregateRoot
{
    private bool _active;
    private string _author;
    private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    public PostAggregate()
    {

    }

    public PostAggregate(Guid id, string author, string content)
    {
        RaiseEvent(new PostCreatedEvent
        {
            Id = id,
            Author = author,
            Content = content,
            DatePosted = DateTime.Now
        });
    }

    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        _author = @event.Author;
        _active = true;
    }

    public void EditContent(string content)
    {
        if(!_active)
        {
            throw new InvalidOperationException("You cannot edit a post that has been deleted.");
        }

        if(string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidOperationException($"The value for '{nameof(content)}' cannot be null or whitespace. Please provide a valid {nameof(content)}.");
        }

        RaiseEvent(new PostUpdatedEvent
        {
            Id = _id,
            Content = content
        });
    }

    public void Apply(PostUpdatedEvent @event)
    {
        _id = @event.Id;
    }

    public void LikePost() // Method to like a post
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot like a post that has been deleted.");
        }

        RaiseEvent(new PostLikedEvent
        {
            Id = _id
        });
    }

    public void Apply(PostLikedEvent @event) // Apply method for handling the PostLikedEvent
    {
        _id = @event.Id;
    }

    public void AddComment(string comment, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot add a comment to a post that has been deleted.");
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException($"The value for '{nameof(comment)}' cannot be null or whitespace. Please provide a valid {nameof(comment)}.");
        }

        RaiseEvent(new CommentAddedEvent
        {
            Id = _id,
            CommentId = Guid.NewGuid(),
            Comment = comment,
            Username = username,
            CommentDate = DateTime.Now
        });
    }

    public void Apply(CommentAddedEvent @event) // Apply method for handling the CommentAddedEvent
    {
        _id = @event.Id;
        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
    }

    public void EditComment(Guid commentId, string newComment, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot edit a comment on a post that has been deleted.");
        }

        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You cannot edit another user's comment.");
        }

        RaiseEvent(new CommentUpdatedEvent
        {
            Id = _id,
            CommentId = commentId,
            Comments = newComment,
            Username = username,
            DateUpdated = DateTime.Now
        });
    }

    public void Apply(CommentUpdatedEvent @event) // Apply method for handling the CommentUpdatedEvent
    {
        _id = @event.Id;
        _comments[@event.CommentId] = new Tuple<string, string>(@event.Comments, @event.Username);
    }

    public void DeleteComment(Guid commentId, string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot delete a comment on a post that has been deleted.");
        }

        if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You cannot delete another user's comment.");
        }

        RaiseEvent(new CommentRemovedEvent
        {
            Id = _id,
            CommentId = commentId
        });
    }

    public void Apply(CommentRemovedEvent @event) // Apply method for handling the CommentRemovedEvent
    {
        _id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

    public void DeletePost(string username)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot delete a post that has already been deleted.");
        }

        if (!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException("You cannot delete another user's post.");
        }

        RaiseEvent(new PostDeletedEvent
        {
            Id = _id
        });
    }

    public void Apply(PostDeletedEvent @event) // Apply method for handling the PostDeletedEvent and alter the state of the aggregate accordingly
    {
        _id = @event.Id;
        _active = false;
    }
}
