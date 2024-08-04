using Modisette.Models;
using Modisette.Services;

namespace Modisette.Services;

public interface IContactService
{
    Task<bool> ContactExistsAsync(int id);
    Task<IList<Contact>> GetFilteredContactsAsync(string? searchString);
    Task<Contact?> GetContactByIdAsync(int? id);
    Task CreateContactAsync(Contact contact);
    Task UpdateContactAsync(Contact contact);
    Task DeleteContactAsync(Contact contact);
}