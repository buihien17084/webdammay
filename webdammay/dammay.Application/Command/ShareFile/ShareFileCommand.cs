using MediatR;
namespace webdammay.dammay.Application.Command.ShareFile;

public record ShareFileCommand(Guid id, string userId) : IRequest<string>;
