using ContactServiceLibrary.Interfaces;

namespace ContactServiceLibrary.Services;

public class FileService(string filePath) : IFileService
{
    private readonly string _filePath = filePath;

    public string GetContentFromFile()
    {
        throw new NotImplementedException();
    }

    public bool SaveContentToFile(string content)
    {
        throw new NotImplementedException();
    }
}
