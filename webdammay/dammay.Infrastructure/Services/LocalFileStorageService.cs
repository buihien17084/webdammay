using Microsoft.AspNetCore.Http;
using webdammay.dammay.Application.Command.Interfaces;

namespace webdammay.Infrastructure.Services;
public class LocalFileStorageService : IFileStorageService
{
    private readonly string _uploadFolderPath;
    public LocalFileStorageService()
    {
        _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
        if (!Directory.Exists(_uploadFolderPath))
        {
            Directory.CreateDirectory(_uploadFolderPath);
        }
        
    }
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0) throw new ArgumentException("File rỗng.");

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(_uploadFolderPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }
    public Task<Stream> GetFileStreamAsync(string filePath)
    {
        if(!File.Exists(filePath))
            throw new FileNotFoundException("Không tìm thấy tệp tin trên hệ thống lưu trữ.");
        Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return Task.FromResult(stream);
    }
   
        public Task DeleteFileAsync(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        return Task.CompletedTask;
    }

}




