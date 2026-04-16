using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Repository.Service;

public class FileStorageService : IFileStorageService
{
    private static readonly HashSet<string> ImageExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

    private static readonly HashSet<string> VideoExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".mp4", ".webm", ".mov" };

    private const long ImageMaxBytes = 10L * 1024 * 1024;       // 10 MB
    private const long VideoMaxBytes = 500L * 1024 * 1024;      // 500 MB

    private static readonly Regex SafeFolder = new("^[A-Za-z0-9_-]+$", RegexOptions.Compiled);

    private static string UploadRoot => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

    public async Task<string> SaveFileAsync(IFormFile file, string folder, UploadKind kind)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Empty file.", nameof(file));

        if (string.IsNullOrWhiteSpace(folder) || !SafeFolder.IsMatch(folder))
            throw new ArgumentException("Invalid folder name.", nameof(folder));

        var (allowed, maxBytes) = kind switch
        {
            UploadKind.Image => (ImageExtensions, ImageMaxBytes),
            UploadKind.Video => (VideoExtensions, VideoMaxBytes),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };

        if (file.Length > maxBytes)
            throw new ArgumentException($"File exceeds the {maxBytes / (1024 * 1024)} MB limit.", nameof(file));

        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(ext) || !allowed.Contains(ext))
            throw new ArgumentException($"Unsupported file extension '{ext}'.", nameof(file));

        var uploadDir = Path.Combine(UploadRoot, folder);
        Directory.CreateDirectory(uploadDir);

        var newFileName = $"{Guid.NewGuid()}{ext.ToLowerInvariant()}";
        var filePath = Path.Combine(uploadDir, newFileName);

        await using (var fs = new FileStream(filePath, FileMode.CreateNew))
        await using (var source = file.OpenReadStream())
        {
            await source.CopyToAsync(fs);
        }

        return $"/{folder}/{newFileName}";
    }

    public Task DeleteFileAsync(string storedPath)
    {
        if (string.IsNullOrWhiteSpace(storedPath))
            return Task.CompletedTask;

        // Only accept the relative form that SaveFileAsync returns: "/folder/filename.ext".
        // Anything else (absolute path, traversal, extra segments) is refused.
        if (!storedPath.StartsWith('/') || storedPath.Contains("..", StringComparison.Ordinal))
            return Task.CompletedTask;

        var segments = storedPath.TrimStart('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length != 2 || !SafeFolder.IsMatch(segments[0]))
            return Task.CompletedTask;

        var rootFull = Path.GetFullPath(UploadRoot);
        var target = Path.GetFullPath(Path.Combine(rootFull, segments[0], segments[1]));

        // Defence in depth: verify the resolved path is still inside the upload root.
        if (!target.StartsWith(rootFull + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            return Task.CompletedTask;

        if (File.Exists(target))
            File.Delete(target);

        return Task.CompletedTask;
    }
}
