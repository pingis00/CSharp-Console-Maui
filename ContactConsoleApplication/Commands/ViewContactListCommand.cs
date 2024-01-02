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

    public async Task ExecuteAsync()
    {
        try
        {
            _userInterfaceServices.DisplayMenuTitle("View Contact List");

            var serviceResult = await _contactService.GetContactsFromListAsync();
            if (serviceResult.Status == ServiceStatus.SUCCESS)
            {
                if (serviceResult.Result is List<IContact> contacts && contacts.Any())
                {
                    Console.WriteLine("Sort by: \n1. First Name \n2. Last Name \n3. Email \nPress any other key for unsorted");
                    Console.Write("\nChoose an option: ");
                    var sortOption = Console.ReadLine();

                    _userInterfaceServices.ShowContactList("Sorted Contact List", contacts, sortOption);
                }
                else
                {
                    _userInterfaceServices.ShowMessage("There are no contacts in the list.", true);
                }
            }
            else
            {
                _userInterfaceServices.ShowMessage("An error occurred while retrieving the contact list.", true);
            }
        }
        catch (Exception ex)
        {
            _userInterfaceServices.ShowMessage($"An unexpected error occurred while showing the contact list: {ex.Message}", true);
        }
        finally
        {
            _userInterfaceServices.ReturnToMainMenu();
        }
    }
}
