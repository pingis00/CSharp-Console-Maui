namespace ContactServiceLibrary.Interfaces;

public interface IContactService
{
    IServiceResult AddContactToList(IContact contact);
    IServiceResult DeleteContactFromList(string email);
    IServiceResult UpdateContactList(IContact contact);
    IServiceResult GetContactsFromList();
    IServiceResult GetContactByEmailFromList(string email);
}
