using Modisette.Models;

namespace Modisette.Repositories;

public interface ICourseRepository
{
    object CourseDocuments { get; }

    Task AddCourseAsync(Course course);
    Task AddCourseDocumentAsync(CourseDocument courseDocument);
    Task<Course?> GetCourseByCodeAsync(string code);
}
