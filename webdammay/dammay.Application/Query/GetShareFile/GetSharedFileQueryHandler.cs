using MediatR;
using Microsoft.EntityFrameworkCore;
using webdammay.dammay.Application.Command.Interfaces;
using webdammay.dammay.Application.Query.GetFiles;

namespace webdammay.dammay.Application.Query.GetShareFile
{
    public class GetSharedFileQueryHandler: IRequestHandler<GetSharedFileQuery, FileDownloadDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        public GetSharedFileQueryHandler(IApplicationDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<FileDownloadDto> Handle(GetSharedFileQuery request,CancellationToken cancellationToken)
        {
            var file=await _context.StoredFiles.FirstOrDefaultAsync(f=>f.ShareToken==request.token&&f.IsShared,cancellationToken);
            if(file == null)
                throw new KeyNotFoundException("Đường dẫn chia sẻ không tồn tại hoặc đã bị vô hiệu hóa.");
            var stream = await _fileStorageService.GetFileStreamAsync(file.FilePath);
            return new FileDownloadDto(stream, file.FileName, "application/octet-stream");
        }
    }
}
