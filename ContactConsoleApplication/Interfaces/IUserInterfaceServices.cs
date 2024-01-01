using ContactServiceLibrary.Interfaces;

namespace ContactConsoleApplication.Interfaces;

public interface IUserInterfaceServices
{
    void ExitApplication();
    void DisplayMenuTitle(string title);
    string ReadNonEmptyInput(string prompt);
    void ReturnToMainMenu();
    void ShowMessage(string message, bool isError, Exception? ex = null);
    bool AskToContinue(string message);
    void ShowContactList(string title, List<IContact> contacts, string? sortOption = "");
    void ShowContactDetails(IContact contact, string title);
    string ReadValidPhoneNumber(string prompt, bool allowEmpty = false);
    string ReadValidEmail(string prompt);
    IContact GetUserSelectedContact(List<IContact> contacts, string prompt);
}
