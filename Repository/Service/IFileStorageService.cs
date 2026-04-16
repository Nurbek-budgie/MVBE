using Microsoft.AspNetCore.Http;

namespace Repository.Service;

public enum UploadKind
{
    Image,
    Video
}

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string folder, UploadKind kind);
    Task DeleteFileAsync(string storedPath);
}
