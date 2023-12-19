using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;

namespace ContactConsoleApplication.Commands;

public class ViewContactListCommand : ICommand
{
    private readonly IContactService _contactService;
    private readonly IUserInterfaceServices _userInterfaceServices;

    public ViewContactListCommand(IContactService contactService, IUserInterfaceServices userInterfaceServices)
    {
        _contactService = contactService;
        _userInterfaceServices = userInterfaceServices;
    }

    public void Execute()
    {
        _userInterfaceServices.DisplayMenuTitle("Show Contact List");

        Console.WriteLine("Sort by: \n1. First Name \n2. Last Name \n3. Email \nPress any other key for unsorted");
        Console.Write("\nChoose an option: ");
        var sortOption = Console.ReadLine();

        var serviceResult = _contactService.GetContactsFromList();

        if (serviceResult.Status == ServiceStatus.SUCCESS)
        {
            if (serviceResult.Result is List<IContact> contacts && contacts.Any())
            {
                var (sortedContacts, sortMethod) = _userInterfaceServices.SortContacts(contacts, sortOption!);
                _userInterfaceServices.ShowContactList("Contact List", sortedContacts, sortMethod);
            }
            else
            {
                _userInterfaceServices.ShowMessage("There are no contacts in the list.", isError: true);
            }
        }
        else
        {
            _userInterfaceServices.ShowMessage("An error occurred while retrieving the contact list.", isError: true);
        }

        _userInterfaceServices.ReturnToMainMenu();
    }
}
