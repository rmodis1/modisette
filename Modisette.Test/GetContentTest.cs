using Moq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modisette.Pages;
using Modisette.Services;
using Modisette.Models;

[TestClass]
public class GetContentTest
{
    private Mock<ICourseService> _courseServiceMock;
    private ContentModel _pageModel;

    [TestInitialize]
    public void Setup()
    {
        _courseServiceMock = new Mock<ICourseService>();

        _courseServiceMock.Setup(service => service.GetYearsAsync())
            .ReturnsAsync(new List<SelectListItem>
            {
                new SelectListItem { Value = "2023", Text = "2023" },
                new SelectListItem { Value = "2022", Text = "2022" }
            });

        _courseServiceMock.Setup(service => service.GetSemestersAsync(2023))
            .ReturnsAsync(new List<SelectListItem>
            {
                new SelectListItem { Value = "Spring", Text = "Spring" },
                new SelectListItem { Value = "Fall", Text = "Fall" }
            });

        _courseServiceMock.Setup(service => service.GetCourseCodesAsync(2023, TimeOfYear.Fall))
            .ReturnsAsync(new List<SelectListItem>
            {
                new SelectListItem { Value = "CS101", Text = "CS101" },
                new SelectListItem { Value = "CS102", Text = "CS102" }
            });

        _courseServiceMock.Setup(service => service.GetCourseDocumentsAsync("CS101"))
            .ReturnsAsync(new List<CourseDocument>
            {
                new CourseDocument { CourseCode = "CS101", Name = "Document1", Document = new Uri("document.pdf", UriKind.RelativeOrAbsolute) }
            });

        _pageModel = new ContentModel(_courseServiceMock.Object);
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
        Assert.AreEqual(2, _pageModel.Semesters.Count);
        Assert.AreEqual(2, _pageModel.CourseCodes.Count);
        Assert.AreEqual(1, _pageModel.CourseDocuments.Count);
    }
}