using cqrs.core.Commands;

namespace Post.Command.Api.Commands;

public class UpdateCommentCommand : BaseCommand
{
    public Guid CommentId { get; set; }
    public string Comment { get; set; }
    public string Usernmae { get; set; }
}
