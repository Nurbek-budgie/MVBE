namespace Repository.Service;

public class FileStorageService : IFileStorageService
{
    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder)
    {
        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);

        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }
        
        var newFileName = Path.Combine($"{Guid.NewGuid()}{Path.GetExtension(fileName)}"); // 12ds-2dasq-dwd.(img,pdf,csv)
        var filePath = Path.Combine(uploadDir, newFileName);

        using (var fileStreamFileStream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(fileStreamFileStream);
        }
        
        return $"/{folder}/{newFileName}";
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