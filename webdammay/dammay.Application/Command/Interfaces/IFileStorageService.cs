using Microsoft.AspNetCore.Http;

namespace webdammay.dammay.Application.Command.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file);
    Task<Stream> GetFileStreamAsync(string filePath);
}