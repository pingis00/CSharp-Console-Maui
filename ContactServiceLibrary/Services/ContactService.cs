using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Models.Responses;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ContactServiceLibrary.Services;

public class ContactService : IContactService
{
    private readonly IFileService _fileService;

    public ContactService(IFileService fileService)
    {
        _fileService = fileService;
    }

    private List<IContact> _contacts = [];


    public IServiceResult AddContactToList(IContact contact)
    {
        var response = new ServiceResult();

        try
        {
            if (_contacts.Any(x => x.Email == contact.Email))
            {
                response.Status = Enums.ServiceStatus.ALREADY_EXISTS;
            }
            else
            {
                _contacts.Add(contact);
                string json = JsonConvert.SerializeObject(_contacts, Formatting.Indented);
                bool result = _fileService.SaveContentToFile(@"c:\School\CSharp-Projects\Json-filer\contacts.json", json);

                response.Status = result ? Enums.ServiceStatus.SUCCESS : Enums.ServiceStatus.FAILED;
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
        try
        {

        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
    }

    public IServiceResult GetContactByEmailFromList(string email)
    {
        try
        {
            
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
    }

    public IServiceResult GetContactsFromList()
    {
        var response = new ServiceResult();

        try
        {
            response.Status = Enums.ServiceStatus.SUCCESS;
            response.Result = _contacts;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = Enums.ServiceStatus.FAILED;
        }

        return response;
    }

    public IServiceResult UpdateContactList(IContact contact)
    {
        try
        {

        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
    }
}
