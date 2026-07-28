using CatalogService.Application.Interfaces.Storage;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CatalogService.Infrastructure.Storage
{
    public sealed class LocalFileStorage : ILocalFileStorage
    {
        private readonly string _basePath;
        private readonly LocalFileStorageOptions _options;

        public LocalFileStorage(IOptions<LocalFileStorageOptions> options)
        {
            _options = options.Value;
            _basePath = options.Value.BasePath ?? throw new ArgumentNullException(nameof(options), "BasePath cannot be null.");
        }

        public async Task<string> StoreFileAsync(Stream fileStream, string fileName, string? subPath = null, CancellationToken cancellationToken = default)
        {
            // Validate & sanitize subPath
            var sanitizedSubPath = SanitizeSubPath(subPath);
            var directoryPath = Path.Combine(_basePath, sanitizedSubPath);
            Directory.CreateDirectory(directoryPath);

            // Tạo unique filename để tránh ghi đè + giữ extension gốc
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(directoryPath, uniqueFileName);

            // Ghi file an toàn
            await using var outputFileStream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            await fileStream.CopyToAsync(outputFileStream, cancellationToken);

            // Trả về relative path đã normalize
            return Path.Combine(sanitizedSubPath, uniqueFileName).Replace('\\', '/');
        }

        private static string SanitizeSubPath(string? subPath)
        {
            if (string.IsNullOrWhiteSpace(subPath))
                return string.Empty;

            // Loại bỏ path traversal
            var normalized = subPath.Replace('\\', '/')
                                    .Replace("../", "")
                                    .Replace("..\\", "")
                                    .Trim('/');

            // Chỉ cho phép ký tự an toàn
            if (!Regex.IsMatch(normalized, @"^[a-zA-Z0-9_\-/]+$"))
                throw new ArgumentException($"Invalid subPath: '{subPath}'");

            return normalized;
        }

        public async Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var fullPath = GetFullLocalPath(filePath);
            if (File.Exists(fullPath))
            {
                try
                {
                    await Task.Run(() => File.Delete(fullPath), cancellationToken); // File.Delete là blocking, wrap vào Task.Run nếu cần truly async
                    return true;
                }
                catch (Exception)
                {
                    // Log exception nếu cần thiết
                    return false;
                }
            }
            return false; // File không tồn tại
        }

        public string GetFullLocalPath(string relativeFilePath)
        {
            // Đảm bảo relativeFilePath không cố gắng truy cập ngoài thư mục gốc (path traversal protection)
            var normalizedRelativePath = relativeFilePath.Replace('/', Path.DirectorySeparatorChar).Replace("..\\", "").Replace("../", "");
            return Path.Combine(_basePath, normalizedRelativePath);
        }

        public async Task<string> StoreImageAsync(IFormFile imageFile, string subPath)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return string.Empty;
            }

            // Kiểm tra loại file
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException($"File type '{extension}' is not allowed.");
            }

            using (var stream = imageFile.OpenReadStream())
            {
                return await StoreFileAsync(stream, imageFile.FileName, subPath);
            }
        }
    }
}
