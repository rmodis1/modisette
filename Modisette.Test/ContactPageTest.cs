using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modisette.Models;
using Modisette.Data;
using Modisette.Pages;
using Microsoft.EntityFrameworkCore;

namespace Modisette.Tests
{
    [TestClass]
    public class ContactPageModelTest
    {
        private Mock<IEmailService> _mockEmailService;
        private Mock<EmailAddress> _mockEmailAddress;
        private SiteContext _context;
        private ContactModel _contactPageModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockEmailService = new Mock<IEmailService>();
            _mockEmailAddress = new Mock<EmailAddress>();

            // Use an in-memory database for testing
            var options = new DbContextOptionsBuilder<SiteContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new SiteContext(options);

            _contactPageModel = new ContactModel(_context, _mockEmailAddress.Object, _mockEmailService.Object)
            {
                Contact = new Contact
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "555-555-5555",
                    Message = "Hello!"
                }
            };
        }

        [TestMethod]
        public async Task OnPostAsync_ModelStateValid_EmailSent()
        {
            // Arrange
            _contactPageModel.ModelState.Clear();

            // Act
            var result = await _contactPageModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirectToPageResult = result as RedirectToPageResult;
            Assert.AreEqual("./Index", redirectToPageResult.PageName);

            _mockEmailService.Verify(service => service.Send(It.IsAny<EmailMessage>()), Times.Once);
            Assert.AreEqual(1, await _context.Contact.CountAsync());
        }
    }
}