namespace Repository.Service;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream fileSteam, string fileName, string folder);
    Task DeleteFileAsync(string filePath);
}