using MediatR;
using Microsoft.EntityFrameworkCore;
using webdammay.dammay.Domain.Entities;
using webdammay.dammay.Application.Command.Interfaces;

namespace webdammay.dammay.Application.Command.DeleteFile
{
    public class DeleteFileCommandHandler:IRequestHandler<DeleteFile,FileDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        public DeleteFileCommandHandler (IApplicationDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public Task<FileDto> Handle(DeleteFile request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<FileDto> Handler (DeleteFile request,CancellationToken cancellationToken)
        {
            var file= await _context.StoredFiles.FirstOrDefaultAsync(f=>f.Id == request.Id&&f.UserId==request.userId,cancellationToken);
            if (file == null)
                throw new KeyNotFoundException("Không tìm thấy file hoặc bạn không có quyền xóa file này.");
            await _fileStorageService.DeleteFileAsync(file.FilePath);
            _context.StoredFiles.Remove(file);
            await _context.SaveChangesAsync(cancellationToken);
            return new FileDto(file.Id, file.FileName,file.FileSize,file.CreatedAt);
        }
    }
}
