using Pickuplay.Services;

class StorageService : IStorageService
{
    public string SaveFile(IFormFile file, string fileName, string folderName)
    {
        var extension = Path.GetExtension(file.FileName);

        var folderPath = Path.Combine("/uploads", folderName);

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, $"{fileName}{extension}");

        using(var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return extension;

    }
}