using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modisette.Data;
using Modisette.Models;
using System.Threading.Tasks;
using modisette.Pages.ContactForm;
using Moq;
using Modisette.Services;

namespace Modisette.Tests
{
    [TestClass]
    public class DeletePageTests
    {
        private Mock<IContactService>? _mockContactService;
        private DeleteModel? _pageModel;

        [TestInitialize]
        public void Setup()
        {
             _mockContactService = new Mock<IContactService>();

            var contacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "555-555-5555", Message = "Hello!" },
                new Contact { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", PhoneNumber = "123-456-7890", Message = "Howdy!" }
            };

            _mockContactService.Setup(service => service.GetContactByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => contacts.Find(c => c.Id == id));

            _mockContactService.Setup(service => service.DeleteContactAsync(It.IsAny<Contact>()))
                .Callback<Contact>(contact => contacts.RemoveAll(c => c.Id == contact.Id))
                .Returns(Task.CompletedTask);

            _pageModel = new DeleteModel(_mockContactService.Object);
        }

        [TestMethod]
        public async Task OnPostAsync_ValidId_ShouldDeleteContact()
        {
            // Arrange
            int validId = 1;

            // Act
            var result = await _pageModel.OnPostAsync(validId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.AreEqual("./Display", ((RedirectToPageResult)result).PageName);
            _mockContactService.Verify(service => service.DeleteContactAsync(It.IsAny<Contact>()), Times.Once);
            _mockContactService.Verify(service => service.GetContactByIdAsync(validId), Times.Once);
        }

        [TestMethod]
        public async Task OnPostAsync_NullId_ShouldReturnNotFound()
        {
            // Act
            var result = await _pageModel.OnPostAsync(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}