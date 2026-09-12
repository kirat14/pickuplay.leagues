namespace Pickuplay.Services;

public interface IStorageService
{
    /// <summary>
    /// Saves an uploaded file to the specified directory using a unique identifier.
    /// </summary>
    /// <param name="file">The uploaded file to be stored.</param>
    /// <param name="folderName">The target subfolder where the file will be saved.</param>
    /// <returns>The generated unique file path relative to the base storage folder.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is null or empty.</exception>
    Task<string?> SaveFile(IFormFile file, string folderName);

    /// <summary>
    /// Resolves the full public URL for a given relative file path.
    /// </summary>
    /// <param name="relativeFilePath">The relative path stored in the database.</param>
    /// <returns>The fully qualified URL string, or null if empty.</returns>
    string? GetFileUrl(string relativeFilePath);
}