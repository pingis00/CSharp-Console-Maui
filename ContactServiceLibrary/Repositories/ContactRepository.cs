using ContactServiceLibrary.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ContactServiceLibrary.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly IFileService _fileService;
    private readonly string _filepath;

    /// <summary>
    /// Initializes a new instance of the ContactRepository class.
    /// </summary>
    /// <param name="fileService">The file service to handle file operations</param>
    /// <param name="filepath">The path to the file where contacts are stored</param>
    public ContactRepository(IFileService fileService, string filepath)
    {
        _fileService = fileService;
        _filepath = filepath;
    }

    public async Task<List<IContact>> LoadContactsAsync()
    {
        try
        {
            var json = await _fileService.GetContentFromFileAsync(_filepath);
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

                return JsonConvert.DeserializeObject<List<IContact>>(json, settings) ?? new List<IContact>();
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new List<IContact>();
    }

    public async Task SaveContactsAsync(List<IContact> contacts)
    {
        try
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented
            };

            var json = JsonConvert.SerializeObject(contacts, settings);
            await _fileService.SaveContentToFileAsync(_filepath, json);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message);}
    }
}
