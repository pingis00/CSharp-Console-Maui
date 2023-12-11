using ContactServiceLibrary.Interfaces;

namespace ContactServiceLibrary.Services;

public class ContactService : IContactService
{
    public IServiceResult AddContactToList(IContact contact)
    {
        throw new NotImplementedException();
    }

    public IServiceResult DeleteContactFromList(string email)
    {
        throw new NotImplementedException();
    }

    public IServiceResult GetCustomerByEmailFromList(string email)
    {
        throw new NotImplementedException();
    }

    public IServiceResult GetCustomersFromList()
    {
        throw new NotImplementedException();
    }

    public IServiceResult UpdateContactList(IContact contact)
    {
        throw new NotImplementedException();
    }
}
