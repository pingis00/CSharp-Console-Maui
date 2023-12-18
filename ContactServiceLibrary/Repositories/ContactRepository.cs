using ContactServiceLibrary.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ContactServiceLibrary.Repositories;

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
        try
        {
            var json = _fileService.GetContentFromFile(_filepath);
            if (string.IsNullOrEmpty(json))
            {
                return new List<IContact>();
            }
            else
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };

                return JsonConvert.DeserializeObject<List<IContact>>(json, settings)!;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public void SaveContacts(List<IContact> contacts)
    {
        try
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented
            };

            var json = JsonConvert.SerializeObject(contacts, settings);
            _fileService.SaveContentToFile(_filepath, json);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message);}
    }
}
