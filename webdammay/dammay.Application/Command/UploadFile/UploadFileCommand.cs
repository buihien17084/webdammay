using MediatR;
using Microsoft.AspNetCore.Http;

namespace  webdammay.dammay.Application.Command.UploadFile;

public record UploadFileCommand(IFormFile File, string UserId) : IRequest<FileDto>;
public record FileDto(Guid Id, string FileName, long FileSize, DateTime CreatedAt);