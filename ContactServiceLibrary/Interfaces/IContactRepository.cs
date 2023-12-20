namespace ContactServiceLibrary.Interfaces;

public interface IContactRepository
{
    /// <summary>
    /// Saves a list of contacts to a persistent storage.
    /// </summary>
    /// <param name="contacts">The list of contacts to be saved</param>
    void SaveContacts(List<IContact> contacts);

    /// <summary>
    /// Loads a list of contacts from a persistent storage.
    /// </summary>
    /// <returns>A list of contacts</returns>
    List<IContact> LoadContacts();
}
