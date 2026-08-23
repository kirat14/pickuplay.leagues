using Pickuplay.Services;

class StorageService : IStorageService
{
    private readonly string _uploadsPath;

    public StorageService(IConfiguration configuration)
    {
        _uploadsPath = configuration["Storage:UploadsPath"] 
            ?? throw new InvalidOperationException("Storage:The UploadsPath not configured.");
    }

    public async Task<string> SaveFile(IFormFile file, string fileName, string folderName)
    {
        var extension = Path.GetExtension(file.FileName);

        var folderPath = Path.Combine(_uploadsPath, folderName);

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, $"{fileName}{extension}");

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return extension;

    }
}