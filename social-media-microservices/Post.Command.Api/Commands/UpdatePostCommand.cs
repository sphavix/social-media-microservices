using cqrs.core.Commands;

namespace Post.Command.Api.Commands;

public class UpdatePostCommand : BaseCommand
{
    public string Content { get; set; }
}
