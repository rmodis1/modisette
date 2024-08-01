using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modisette.Data;
using Modisette.Models;
using Modisette.Pages.Admin.ContactForm;
using Modisette.Pages.ContactForm;

namespace Modisette.Tests
{
    [TestClass]
    public class SearchStringTest
    {
        private SiteContext _context;
        private DisplayModel _pageModel;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SiteContext>()
                .UseInMemoryDatabase(databaseName: "MockDb")
                .Options;

            _context = new SiteContext(options);

            // Seed the in-memory database with test data
            _context.Contact.AddRange(new List<Contact>
            {
                new Contact { FirstName = "John", LastName = "Doe", Email = "fastdriver@someaddress.com", PhoneNumber = "999-999-9999", Message = "Hello", Notes = "Note1" },
                new Contact { FirstName = "Jane", LastName = "Doe", Email = "slowdriver@someaddress.com", PhoneNumber = "777-777-7777", Message = "Howdy", Notes = "Note2" },
                new Contact { FirstName = "Alice", LastName = "Smith", Email = "a.smith@someaddress.com", PhoneNumber = "222-222-2222", Message = "Hey", Notes = "Note3" }
            });

            _context.SaveChanges();

            _pageModel = new DisplayModel(_context);
        }

         [TestMethod]
        public async Task OnGetAsync_WithSearchString_ShouldReturnMatchingContacts()
        {
            // Arrange
            _pageModel.SearchString = "Smith";

            // Act
            await _pageModel.OnGetAsync();

            // Assert
            foreach (var contact in _pageModel.Contacts)
            {
                Console.WriteLine(contact.FirstName);
            }
            Assert.AreEqual(1, _pageModel.Contacts.Count);
            Assert.IsTrue(_pageModel.Contacts.Any(c => c.FirstName == "Alice" && c.LastName == "Smith"));
        }
    }
}