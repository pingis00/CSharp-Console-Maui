namespace ContactServiceLibrary.Interfaces;

public interface IContactRepository
{
    void SaveContacts(List<IContact> contacts);
    List<IContact> LoadContacts();
}
