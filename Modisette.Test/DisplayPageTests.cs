using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modisette.Data;
using Modisette.Models;
using Modisette.Pages.Admin.ContactForm;
using Modisette.Pages.ContactForm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modisette.Tests
{
    [TestClass]
    public class DisplayPageTests
    {
        private SiteContext _context;
        private DisplayModel _pageModel;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SiteContext>()
                .UseInMemoryDatabase(databaseName: "PretendDb")
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
        public async Task OnGetAsync_NoSearchString_ShouldReturnAllContacts()
        {
            // Arrange
            _pageModel.SearchString = string.Empty;

            // Act
            await _pageModel.OnGetAsync();

            // Assert
            foreach (var contact in _pageModel.Contacts)
            {
                Console.WriteLine(contact.FirstName);
            }
            Assert.AreEqual(3, _pageModel.Contacts.Count);
        }

        [TestMethod]
        public async Task OnGetAsync_WithSearchString_ShouldReturnNoContacts()
        {
            // Arrange
            _pageModel.SearchString = "NonExistent";

            // Act
            await _pageModel.OnGetAsync();

            // Assert
            Assert.AreEqual(0, _pageModel.Contacts.Count);
        }
    }
}