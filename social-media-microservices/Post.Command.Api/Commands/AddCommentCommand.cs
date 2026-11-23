using cqrs.core.Commands;

namespace Post.Command.Api.Commands;

public class AddCommentCommand : BaseCommand
{
    public string Comment { get; set; }
    public string Username { get; set; }
}
