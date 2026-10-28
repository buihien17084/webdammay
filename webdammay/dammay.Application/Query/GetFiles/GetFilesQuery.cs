using MediatR;
namespace webdammay.dammay.Application.Query.GetFiles
{
    public record GetFilesQuery(string userId) : IRequest<List< FileDto>>;
    public record FileDto(Guid Id, string FileName, long FileSize, DateTime CreatedAt);
}
