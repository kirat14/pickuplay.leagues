using Pickuplay.Services;

class StorageService : IStorageService
{
    private readonly string _uploadsPath;
    private readonly string _baseUrl;

    public StorageService(IConfiguration configuration)
    {
        _uploadsPath = configuration["Storage:UploadsPath"]
            ?? throw new InvalidOperationException("The UploadsPath not configured.");
        _baseUrl = configuration["Storage:BaseUrl"]
            ?? throw new InvalidOperationException("The BaseUrl not configured.");
    }

    public async Task<string?> SaveFile(IFormFile file, string folderName)
    {
        var extension = Path.GetExtension(file.FileName);

        var folderPath = Path.Combine(_uploadsPath, folderName);
        var fileName = $"{Guid.NewGuid()}{extension}";

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, $"{fileName}");

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"{folderName}/{fileName}";
    }

    public string? GetFileUrl(string? filePath) => !string.IsNullOrEmpty(filePath) ? $"{_baseUrl}/{filePath}" : null;
}