using ContactServiceLibrary.Interfaces;
using Newtonsoft.Json;

namespace ContactServiceLibrary.Services;

public class ContactRepository : IContactRepository
{
    private readonly IFileService _fileService;
    private readonly string _filepath;

    public ContactRepository(IFileService fileService, string filepath)
    {
        _fileService = fileService;
        _filepath = filepath;
    }

    public List<IContact> LoadContacts()
    {
        var json = _fileService.GetContentFromFile(_filepath);
        if (string.IsNullOrEmpty(json))
        {
            return new List<IContact>();
        }
        else
        {
            return JsonConvert.DeserializeObject<List<IContact>>(json)!;
        }
    }

    public void SaveContacts(List<IContact> contacts)
    {
        var json = JsonConvert.SerializeObject(contacts, Formatting.Indented);
        _fileService.SaveContentToFile(_filepath, json);
    }
}
