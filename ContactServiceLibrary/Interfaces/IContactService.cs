namespace ContactServiceLibrary.Interfaces;

public interface IContactService
{
    IServiceResult AddContact(IContact contact);
    IServiceResult DeleteContact(string email);
    IServiceResult UpdateContact(IContact contact);
    IServiceResult GetContactsFromList();
    IServiceResult GetContactByEmailFromList(string email);
}
