namespace Pickuplay.Services;
public interface IStorageService
{
    // Saves the file to disk and returns the file extension
    string SaveFile(IFormFile file, long id, string folderName);
}