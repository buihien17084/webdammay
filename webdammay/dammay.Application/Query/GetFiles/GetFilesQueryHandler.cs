using MediatR;

using Microsoft.EntityFrameworkCore;
using webdammay.dammay.Application.Command.Interfaces;

namespace webdammay.dammay.Application.Query.GetFiles
{
    public class GetFilesQueryHandler: IRequestHandler<GetFilesQuery,List< FileDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetFilesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List< FileDto>> Handle(GetFilesQuery request,CancellationToken cancellationToken)
        {
            var files = await _context.StoredFiles
                .Where(f => f.UserId == request.userId)
                .Select(f => new FileDto(f.Id, f.FileName, f.FileSize, f.CreatedAt))
                .ToListAsync(cancellationToken);

            return files;
        } 

    }
}
