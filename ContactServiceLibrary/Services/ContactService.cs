using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Models.Responses;
using System.Diagnostics;

namespace ContactServiceLibrary.Repositories;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public IServiceResult AddContactToList(IContact contact)
    {
        var response = new ServiceResult();

        try
        {
            var contacts = _contactRepository.LoadContacts();
            if (contacts.Any(x => x.Email == contact.Email))
            {
                response.Status = Enums.ServiceStatus.ALREADY_EXISTS;
            }
            else
            {
                contacts.Add(contact);
                _contactRepository.SaveContacts(contacts);

                response.Status = Enums.ServiceStatus.SUCCESS;
            }
        }
        catch (Exception ex)
        { 
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILED;
        }

        return response;
    }

    public IServiceResult DeleteContactFromList(string email)
    {
        return new ServiceResult
        {
            Status = ServiceStatus.SUCCESS,
            Result = null!
        };
    }

    public IServiceResult GetContactByEmailFromList(string email)
    {
        return new ServiceResult
        {
            Status = ServiceStatus.SUCCESS,
            Result = null!
        };
    }

    public IServiceResult GetContactsFromList()
    {
        return new ServiceResult
        {
            Status = ServiceStatus.SUCCESS,
            Result = null!
        };
    }

    public IServiceResult UpdateContactList(IContact contact)
    {
        return new ServiceResult
        {
            Status = ServiceStatus.SUCCESS,
            Result = null!
        };
    }
}
