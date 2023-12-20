namespace ContactServiceLibrary.Interfaces;

public interface IFileService
{
    /// <summary>
    /// Saves content to a specified file.
    /// </summary>
    /// <param name="filepath">Path to the file where content will be saved</param>
    /// <param name="content">The content to save to the file.</param>
    /// <returns>True if the content was successfully saved, false otherwise</returns>
    bool SaveContentToFile(string filepath, string content);

    /// <summary>
    /// Retrieves content from a specified file.
    /// </summary>
    /// <param name="filepath">Path to the file to read from.</param>
    /// <returns>The content of the file.</returns>
    string GetContentFromFile(string filepath);
}
