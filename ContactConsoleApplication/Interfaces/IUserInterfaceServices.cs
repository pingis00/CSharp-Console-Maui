using ContactServiceLibrary.Interfaces;

namespace ContactConsoleApplication.Interfaces;

public interface IUserInterfaceServices
{
    void ExitApplication();
    void DisplayMenuTitle(string title);
    string ReadNonEmptyInput(string prompt);
    void ReturnToMainMenu();
    void ShowMessage(string message, bool isError);
    bool AskToContinue(string message);
    void ShowContactDetails(IContact contact, string title);
    void ShowContactList(string title, List<IContact> contacts, string sortMethod = "");
    (List<IContact> SortedContacts, string SortMethod) SortContacts(List<IContact> contacts, string sortOption);

    string ReadValidPhoneNumber(string prompt, bool allowEmpty = false);
    string ReadValidEmail(string prompt);

    IContact GetUserSelectedContact(List<IContact> contacts, string prompt);
}
