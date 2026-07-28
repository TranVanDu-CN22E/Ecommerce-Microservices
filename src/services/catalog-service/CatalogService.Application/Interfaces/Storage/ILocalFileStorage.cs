namespace CatalogService.Application.Interfaces.Storage
{
    public interface ILocalFileStorage
    {
        Task<string> StoreFileAsync(Stream fileStream, string fileName, string? subPath = null, CancellationToken cancellationToken = default);
        Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
        string GetFullLocalPath(string relativeFilePath);
        Task<string> StoreImageAsync(IFormFile imageFile, string subPath);
    }
}
