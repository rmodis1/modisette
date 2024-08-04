using Modisette.Models;

namespace Modisette.Services;

public interface IFileService
{
    Task UploadFilesAsync(BufferedFiles files, Course course);
    Task DeleteFileAsync(CourseDocument courseDocument);
    Task<List<CourseDocument>> GetCourseDocumentsAsync(string courseCode);
    Task<List<CourseDocument>> GetCourseDocumentsAsync(Course course);
    Task<CourseDocument> GetCourseDocumentAsync(int fileId);
}