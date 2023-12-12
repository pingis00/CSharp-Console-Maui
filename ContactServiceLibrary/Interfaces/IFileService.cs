namespace ContactServiceLibrary.Interfaces;

public interface IFileService
{
    bool SaveContentToFile(string filepath, string content);
    string GetContentFromFile(string filepath);
}
