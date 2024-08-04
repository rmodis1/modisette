using Modisette.Models;

namespace Modisette.Services;

public interface IFileService
{
    Task UploadFilesAsync(BufferedFiles files, Course course);
    Task<bool> DeleteFileAsync(int fileId);
}