using Microsoft.EntityFrameworkCore;
using Modisette.Data;
using Modisette.Models;

namespace Modisette.Services;

public class ContactService : IContactService
{
    private readonly SiteContext _context;

    public ContactService(SiteContext context)
    {
        _context = context;
    }

    public async Task<bool> ContactExistsAsync(int id)
    {
        return await _context.Contact.AnyAsync(entity => entity.Id == id);
    }

    public async Task<IList<Contact>> GetFilteredContactsAsync(string? searchString)
    {
        var contacts = from contact in _context.Contact
                       select contact;

        if (!string.IsNullOrEmpty(searchString))
        {
            contacts = contacts.Where(contact => contact.FirstName.Contains(searchString) || 
                                                 contact.LastName.Contains(searchString) || 
                                                 contact.Message.Contains(searchString) ||
                                                 contact.Notes.Contains(searchString));
        }

        return await contacts.ToListAsync();
    }

    public async Task<Contact?> GetContactByIdAsync(int? id)
    {
        return await _context.Contact.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task CreateContactAsync(Contact contact)
    {
        _context.Contact.Add(contact);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateContactAsync(Contact contact)
    {
        _context.Attach(contact).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(Contact contact)
    {
        _context.Contact.Remove(contact);
        await _context.SaveChangesAsync();
    }
}