using MediatR;
using Microsoft.EntityFrameworkCore;
using webdammay.dammay.Application.Command.Interfaces;

using webdammay.dammay.Domain.Entities;
namespace webdammay.dammay.Application.Command.UploadFile;

    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileDto>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IApplicationDbContext _context;
    private const long MaxQuotaBytes = 500 * 1024 * 1024;

    public UploadFileCommandHandler(IFileStorageService fileStorageService, IApplicationDbContext context)
    {
        _fileStorageService = fileStorageService;
        _context = context;
    }

   

    public async Task<FileDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        long currentUsedStorage = await _context.StoredFiles
            .Where(f => f.UserId == request.UserId)
            .SumAsync(f => f.FileSize, cancellationToken);

       
        if (currentUsedStorage + request.File.Length > MaxQuotaBytes)
        {
            throw new InvalidOperationException("Đã vượt quá hạn mức dung lượng lưu trữ cho phép (50MB). Không thể tải lên tệp tin này.");
        }
        var filePath = await _fileStorageService.SaveFileAsync(request.File);
        var storedFile = new StoredFile(
        request.File.FileName,
        filePath,
        request.File.Length,
        request.UserId
    );
        _context.StoredFiles.Add(storedFile);
        await _context.SaveChangesAsync(cancellationToken);
        return new FileDto(storedFile.Id, storedFile.FileName, storedFile.FileSize, storedFile.CreatedAt);
    }

}


