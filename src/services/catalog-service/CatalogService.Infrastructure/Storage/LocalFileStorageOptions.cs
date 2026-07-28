namespace CatalogService.Infrastructure.Storage
{
    public sealed record LocalFileStorageOptions
    {
        public const string SectionName = "LocalFileStorage";
        public string BasePath { get; set; } = null!;
    }
}
