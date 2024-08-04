using Modisette.Models;

namespace Modisette.Services;

public interface IContactMessageBuilder
{
    EmailMessage BuildMessage(Contact contact);
}