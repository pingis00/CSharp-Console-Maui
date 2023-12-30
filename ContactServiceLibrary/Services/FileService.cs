using ContactServiceLibrary.Interfaces;
using System.Diagnostics;

namespace ContactServiceLibrary.Repositories;

public class FileService() : IFileService
{
    public async Task<bool> SaveContentToFileAsync(string filepath, string content)
    {
        try
        {
            using var sw = new StreamWriter(filepath, false);
            await sw.WriteLineAsync(content);
            return true;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

    public async Task<string> GetContentFromFileAsync(string filepath)
    {
        try
        {
            if (File.Exists(filepath))
            {
                return await File.ReadAllTextAsync(filepath);
            }
            else
            {
                return string.Empty;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return string.Empty;
    }
}
