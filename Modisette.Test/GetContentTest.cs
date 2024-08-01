using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modisette.Data; 
using Modisette.Models; 
using Modisette.Pages.Admin.ContentForm;
using Modisette.Pages;
using Microsoft.EntityFrameworkCore;

namespace Modisette.Tests
{
    [TestClass]
    public class ContentPageTests
    {
        private SiteContext _context;
        private ContentModel _pageModel;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SiteContext>()
                .UseInMemoryDatabase(databaseName: "MockDatabase")
                .Options;

            _context = new SiteContext(options);

            // Seed the in-memory database with test data
            _context.Courses.AddRange(new List<Course>
            {
                new Course { Year = 2023, Semester = TimeOfYear.Fall, Code = "CS101", Title = "Course1" },
                new Course { Year = 2023, Semester = TimeOfYear.Spring, Code = "CS102", Title = "Course2" },
                new Course { Year = 2022, Semester = TimeOfYear.Summer, Code = "CS103", Title = "Course3" }
            });

            _context.CourseDocuments.AddRange(new List<CourseDocument>
            {
                new CourseDocument { CourseCode = "CS101", Name = "Document1", Document = new Uri("document.pdf", UriKind.RelativeOrAbsolute) },
                new CourseDocument { CourseCode = "CS102", Name = "Document2", Document = new Uri("anotherdocument.pdf", UriKind.RelativeOrAbsolute) }
            });

            _context.SaveChanges();

            _pageModel = new ContentModel(_context);
        }

        [TestMethod]
        public async Task OnGetAsync_ShouldPopulateYearsSemestersCourseCodesAndDocuments()
        {
            // Arrange
            _pageModel.Year = 2023;
            _pageModel.Semester = TimeOfYear.Fall;
            _pageModel.CourseCode = "CS101";

            // Act
            await _pageModel.OnGetAsync();

            // Assert
            Assert.AreEqual(2, _pageModel.Years.Count);
            Assert.AreEqual("2023", _pageModel.Years.First().Value);
            Assert.AreEqual(2, _pageModel.Semesters.Count);
            Assert.AreEqual("Fall", _pageModel.Semesters.First().Value);
            Assert.AreEqual(1, _pageModel.CourseCodes.Count);
            Assert.AreEqual("CS101", _pageModel.CourseCodes.First().Value);
            Assert.AreEqual(1, _pageModel.CourseDocuments.Count);
            Assert.AreEqual("Document1", _pageModel.CourseDocuments.First().Name);
        }
    }
}