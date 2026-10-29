using MediatR;
using webdammay.dammay.Application.Query.GetFiles;

namespace webdammay.dammay.Application.Query.GetShareFile;

public record GetSharedFileQuery(string token): IRequest<FileDownloadDto>;

