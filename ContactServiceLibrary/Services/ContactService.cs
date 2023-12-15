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

    public IServiceResult AddContact(IContact contact)
    {
        var response = new ServiceResult();

        try
        {
            var contacts = _contactRepository.LoadContacts();
            if (contacts.Any(x => x.Email == contact.Email))
            {
                response.Status = ServiceStatus.ALREADY_EXISTS;
            }
            else
            {
                contacts.Add(contact);
                _contactRepository.SaveContacts(contacts);

                response.Status = ServiceStatus.SUCCESS;
            }
        }
        catch (Exception ex)
        { 
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            response.Result = ex.Message;
        }
        return response;
    }

    public IServiceResult DeleteContact(string email)
    {
        var response = new ServiceResult();

        try
        {
            var contacts = _contactRepository.LoadContacts();
            var contactToDelete = contacts.FirstOrDefault(x => x.Email == email);
            if (contactToDelete != null)
            {
                contacts.Remove(contactToDelete);
                _contactRepository.SaveContacts(contacts);
                response.Status = ServiceStatus.DELETED;
            }
            else
            {
                response.Status= ServiceStatus.NOT_FOUND;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            response.Result = ex.Message;
        }
        return response;
    }

    public IServiceResult GetContactByEmailFromList(string email)
    {
        var response = new ServiceResult();

        try
        {
            var contacts = _contactRepository.LoadContacts();
            var getContact = contacts.FirstOrDefault(x => x.Email == email);

            if (getContact != null)
            {
                response.Status = ServiceStatus.SUCCESS;
                response.Result = getContact;
            }
            else
            {
                response.Status = ServiceStatus.NOT_FOUND;
                response.Result = null!;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            response.Result = ex.Message;
        }
        return response;
    }

    public IServiceResult GetContactsFromList()
    {
        var response = new ServiceResult();

        try
        {
            var contacts = _contactRepository.LoadContacts();

            if (contacts != null && contacts.Any())
            {
                response.Status = ServiceStatus.SUCCESS;
                response.Result = contacts;
            }
            else
            {
                response.Status= ServiceStatus.SUCCESS;
                response.Result = new List<IContact>();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            response.Result = ex.Message;
        }
        return response;
    }

    public IServiceResult UpdateContact(IContact contact)
    {
        var response = new ServiceResult();

        try
        {
            var contacts = _contactRepository.LoadContacts();
            var contactToUpdate = contacts.FirstOrDefault(x => x.Email == contact.Email);

            if (contactToUpdate != null)
            {
                contactToUpdate.FirstName = contact.FirstName;
                contactToUpdate.LastName = contact.LastName;
                contactToUpdate.Address = contact.Address;
                contactToUpdate.Email = contact.Email;
                contactToUpdate.PhoneNumber = contact.PhoneNumber;

                _contactRepository.SaveContacts(contacts);
                response.Status = ServiceStatus.UPDATED;
            }
            else
            {
                response.Status = ServiceStatus.NOT_FOUND;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            response.Result = ex.Message;
        }
        return response;
    }
}
