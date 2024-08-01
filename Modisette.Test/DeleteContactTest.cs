using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modisette.Data;
using Modisette.Models;
using System.Threading.Tasks;
using modisette.Pages.ContactForm;

namespace Modisette.Tests
{
    [TestClass]
    public class DeletePageTests
    {
        private SiteContext _context;
        private DeleteModel _pageModel;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SiteContext>()
                .UseInMemoryDatabase(databaseName: "AnotherTestDatabase")
                .Options;

            _context = new SiteContext(options);

            // Seed the in-memory database with test data
            _context.Contact.AddRange(new List<Contact>
            {
                new Contact { FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "555-555-5555", Message = "Hello!" },
                new Contact { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", PhoneNumber = "123-456-7890", Message = "Howdy!" }
            });

            _context.SaveChanges();

            _pageModel = new DeleteModel(_context);
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
            Assert.IsNull(await _context.Contact.FindAsync(validId));
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