using Modisette.Models;
using Modisette.Repositories;

namespace Modisette.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICourseService _courseService;

    public FileService(IWebHostEnvironment webHostEnvironment, ICourseService courseService)
    {
        _webHostEnvironment = webHostEnvironment;
        _courseService = courseService;
    }

    public async Task UploadFilesAsync(BufferedFiles files, Course course)
    {
        foreach (var formFile in files.FormFiles)
        {
            if (formFile.Length > 0)
            {
                var fileName = Path.GetFileName(formFile.FileName);
                var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                var courseDocument = new CourseDocument
                {
                    CourseCode = course.Code,
                    CourseYear = course.Year,
                    CourseSemester = course.Semester,
                    Name = fileName,
                    Document = new Uri(fileName, UriKind.Relative)
                };

                await _courseService.AddCourseDocumentAsync(courseDocument);
            }
        }
    }
    public async Task<bool> DeleteFileAsync(int fileId)
    {
        var file = await _courseService.CourseDocuments.FindAsync(fileId);

        if (file == null)
        {
            return false;
        }

        _courseService.CourseDocuments.Remove(file);
        await _courseService.SaveChangesAsync();

        return true;
    }
}