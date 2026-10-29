using MediatR;
namespace webdammay.dammay.Application.Command.Rename;

    public record RenameFileCommand(Guid FileId, string userId, string NewFileName) : IRequest<Unit>;
    public record RenameFileDto(string NewFileName);


   

