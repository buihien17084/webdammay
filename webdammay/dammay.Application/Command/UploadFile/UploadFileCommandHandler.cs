using MediatR;
using webdammay.dammay.Application.Command.Interfaces;

using webdammay.dammay.Domain.Entities;
namespace webdammay.dammay.Application.Command.UploadFile;

    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileDto>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IApplicationDbContext _context;

    public UploadFileCommandHandler(IFileStorageService fileStorageService, IApplicationDbContext context)
    {
        _fileStorageService = fileStorageService;
        _context = context;
    }

    public Task<FileDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<FileDto> Handler(UploadFileCommand request, CancellationToken cancellationToken)
    {
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


