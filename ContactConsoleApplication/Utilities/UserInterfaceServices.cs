using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Interfaces;

namespace ContactConsoleApplication.Utilities;

public class UserInterfaceServices : IUserInterfaceServices
{
    public bool AskToContinue(string message)
    {
        throw new NotImplementedException();
    }

    public void DisplayMenuTitle(string title)
    {
        throw new NotImplementedException();
    }

    public string ReadNonEmptyInput(string prompt)
    {
        throw new NotImplementedException();
    }

    public void ReturnToMainMenu()
    {
        throw new NotImplementedException();
    }

    public void ShowContactDetails(IContact contact, string title)
    {
        throw new NotImplementedException();
    }

    public void ShowContactList(string title, List<IContact> contacts, string sortMethod = "")
    {
        throw new NotImplementedException();
    }

    public void ShowExitApplicationOption()
    {
        throw new NotImplementedException();
    }

    public void ShowMessage(string message, bool isError)
    {
        throw new NotImplementedException();
    }

    public (List<IContact> SortedContacts, string SortMethod) SortContacts(List<IContact> contacts, string sortOption)
    {
        throw new NotImplementedException();
    }
}
