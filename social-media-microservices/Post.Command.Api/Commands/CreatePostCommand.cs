using cqrs.core.Commands;

namespace Post.Command.Api.Commands;

public class CreatePostCommand : BaseCommand
{
    public string Author { get; set; }
    public string Content { get; set; }
}
