using Microsoft.EntityFrameworkCore;
using Modisette.Data;
using Modisette.Models;

namespace Modisette.Services;

//Single Responsibility Principle (SRP): This class is responsible for handling course files/documents (i.e., file uploads, deletions, and retrievals).
public class FileService : IFileService
{
    private const long MaxFileSizeBytes = 10 * 1024 * 1024;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf",
        ".docx",
        ".doc",
        ".r",
        ".py",
        ".txt",
        ".ppt",
        ".pptx"
    };

    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SiteContext _context;

    public FileService(IWebHostEnvironment webHostEnvironment, SiteContext context)
    {
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    public async Task UploadFilesAsync(BufferedFiles files, Course course)
    {
        if (files.FormFiles == null || files.FormFiles.Count == 0)
        {
            return;
        }

        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
        Directory.CreateDirectory(uploads);

        foreach (var formFile in files.FormFiles)
        {
            if (formFile.Length <= 0)
            {
                continue;
            }

            if (formFile.Length > MaxFileSizeBytes)
            {
                throw new InvalidDataException($"'{formFile.FileName}' exceeds the 10 MB upload limit.");
            }

            var originalFileName = Path.GetFileName(formFile.FileName);
            var extension = Path.GetExtension(originalFileName);
            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            {
                throw new InvalidDataException($"'{originalFileName}' is not an allowed file type.");
            }

            var storedFileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploads, storedFileName);

            await using (var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                await formFile.CopyToAsync(stream);
            }

            var courseDocument = new CourseDocument
            {
                CourseCode = course.Code,
                CourseYear = course.Year,
                CourseSemester = course.Semester,
                Name = originalFileName,
                Document = new Uri(storedFileName, UriKind.Relative)
            };

            await _context.CourseDocuments.AddAsync(courseDocument);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteFileAsync(CourseDocument courseDocument)
    {
        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
        var storedFileName = Path.GetFileName(courseDocument.Document.OriginalString);
        var filePath = Path.Combine(uploads, storedFileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        _context.CourseDocuments.Remove(courseDocument);
        await _context.SaveChangesAsync();
    }


    public async Task<List<CourseDocument>> GetCourseDocumentsAsync(string courseCode)
    {
        return await _context.CourseDocuments
            .Where(cd => cd.CourseCode == courseCode)
            .ToListAsync();
    }

    public async Task<List<CourseDocument>> GetCourseDocumentsAsync(Course course)
    {
        return await _context.CourseDocuments.Where(cd => cd.CourseCode == course.Code && 
                                                   cd.CourseYear == course.Year && 
                                               cd.CourseSemester == course.Semester
                                                        ).ToListAsync();
    }

    public async Task<CourseDocument?> GetCourseDocumentAsync(int fileId)
    {
        return await _context.CourseDocuments.FindAsync(fileId);
    }
}