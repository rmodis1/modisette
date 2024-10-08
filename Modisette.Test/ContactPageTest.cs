using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modisette.Models;
using Modisette.Data;
using Modisette.Pages;
using Microsoft.EntityFrameworkCore;
using Modisette.Services;

namespace Modisette.Tests
{
    [TestClass]
    public class ContactPageModelTest
    {
        private Mock<IEmailService>? _mockEmailService;
        private Mock<IContactMessageBuilder>? _mockContactMessageBuilder;
        private Mock<IContactService>? _mockContactService;
        private ContactModel? _contactPageModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockEmailService = new Mock<IEmailService>();
            _mockContactMessageBuilder = new Mock<IContactMessageBuilder>();
            _mockContactService = new Mock<IContactService>();

            _contactPageModel = new ContactModel(_mockContactService.Object, _mockEmailService.Object, _mockContactMessageBuilder.Object)
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

            var emailMessage = new EmailMessage
            {
                FromEmailAddress = new List<EmailAddress> { new EmailAddress { Address = "test@example.com" } },
                ToEmailAddress = new List<EmailAddress> { new EmailAddress { Address = "test@example.com" } },
                Content = "Test Content",
                Subject = "Test Subject"
            };

            _mockContactMessageBuilder
                .Setup(builder => builder.BuildMessage(It.IsAny<Contact>()))
                .Returns(emailMessage);


            // Act
            var result = await _contactPageModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirectToPageResult = result as RedirectToPageResult;
            Assert.AreEqual("./Index", redirectToPageResult.PageName);

            _mockEmailService.Verify(service => service.Send(It.IsAny<EmailMessage>()), Times.Once);
            _mockContactMessageBuilder.Verify(builder => builder.BuildMessage(It.IsAny<Contact>()), Times.Once);
            _mockContactService.Verify(service => service.CreateContactAsync(It.IsAny<Contact>()), Times.Once);
        }
    }
}