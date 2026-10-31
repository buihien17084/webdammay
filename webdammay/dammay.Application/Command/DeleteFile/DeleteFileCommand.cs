using MediatR;
namespace webdammay.dammay.Application.Command.DeleteFile;

public record DeleteFileCommand(Guid Id, string userId) : IRequest<FileDto>;
public record FileDto(Guid Id, string FileName, long FileSize, DateTime CreatedAt);


