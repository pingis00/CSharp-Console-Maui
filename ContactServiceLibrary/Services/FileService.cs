using ContactServiceLibrary.Interfaces;
using System.Diagnostics;

namespace ContactServiceLibrary.Services;

public class FileService() : IFileService
{
    public bool SaveContentToFile(string filepath, string content)
    {
        try
        {
            using var sw = new StreamWriter(filepath, false);
            sw.WriteLine(content);
            return true;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

    public string GetContentFromFile(string filepath)
    {
        try
        {
            if (File.Exists(filepath))
            {
                return File.ReadAllText(filepath);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
