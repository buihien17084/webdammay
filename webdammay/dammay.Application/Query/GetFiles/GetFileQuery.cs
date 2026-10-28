using MediatR;

namespace webdammay.dammay.Application.Query.GetFiles;
public record GetFileQuery(Guid FileId, string UserId) : IRequest<FileDownloadDto>;

public record FileDownloadDto(Stream FileStream, string FileName, string ContentType);
