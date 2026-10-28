using MediatR;
using Microsoft.EntityFrameworkCore;
using webdammay.dammay.Application.Command.Interfaces;

namespace webdammay.dammay.Application.Query.GetFiles
{
    public class GetFileQueryHandler: IRequestHandler<GetFileQuery,FileDownloadDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        public GetFileQueryHandler (IApplicationDbContext context,IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public Task<FileDownloadDto> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<FileDownloadDto> Handler(GetFileQuery request,CancellationToken cancellationToken)
        {
            var file=await _context.StoredFiles.FirstOrDefaultAsync(f=>f.Id==request.FileId && f.UserId==request.UserId,cancellationToken);
            if(file==null)
                throw new KeyNotFoundException("Không tìm thấy file hoặc bạn không có quyền truy cập.");
            var stream = await _fileStorageService.GetFileStreamAsync(file.FilePath);
            return new FileDownloadDto(stream, file.FileName, "application/octet-stream");

        }
    }
}
