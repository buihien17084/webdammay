using MediatR;
using webdammay.dammay.Application.Command.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace webdammay.dammay.Application.Query.GetFiles
{
    public class GetListFilesQueryHandler:IRequestHandler<GetListFilesQuery, PaginatedListDto<FileListDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetListFilesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<PaginatedListDto<FileListDto>> Handle(GetListFilesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedListDto<FileListDto>> Handler (GetListFilesQuery request,CancellationToken cancellationToken)
        {
            var query = _context.StoredFiles.Where(f => f.UserId == request.Id);
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                query = query.Where(f => f.FileName.ToLower().Contains(term));
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
            .OrderByDescending(f => f.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(f => new FileListDto(f.Id, f.FileName, f.FileSize, f.CreatedAt))
            .ToListAsync(cancellationToken);

            return new PaginatedListDto<FileListDto>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
