using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Modisette.Data;
using Modisette.Models;

namespace Modisette.Services;

//Single Responsibility Principle (SRP): This class is responsible for handling course files/documents (i.e., file uploads, deletions, and retrievals).
public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly SiteContext _context;

    public FileService(IWebHostEnvironment webHostEnvironment, SiteContext context)
    {
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    public async Task UploadFilesAsync(BufferedFiles files, Course course)
    {
        foreach (var formFile in files.FormFiles)
        {
            if (formFile.Length > 0)
            {
                if (formFile.Name.EndsWith(".pdf") 
                || formFile.Name.EndsWith(".docx") 
                || formFile.Name.EndsWith(".doc")
                || formFile.Name.EndsWith(".r")
                || formFile.Name.EndsWith(".py")
                || formFile.Name.EndsWith(".txt")
                || formFile.Name.EndsWith(".ppt")
                || formFile.Name.EndsWith(".pptx"))
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

                    await _context.CourseDocuments.AddAsync(courseDocument);
                }
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

                    await _context.CourseDocuments.AddAsync(courseDocument);
                }
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

                    await _context.CourseDocuments.AddAsync(courseDocument);
                }
            }
        }
        await _context.SaveChangesAsync();
    }
    public async Task DeleteFileAsync(CourseDocument courseDocument)
    {
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

    public async Task<CourseDocument> GetCourseDocumentAsync(int fileId)
    {
        return await _context.CourseDocuments.FindAsync(fileId);
    }
}