using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;

namespace ContactConsoleApplication.Commands;

public class ViewContactDetailCommand : ICommand
{
    private readonly IContactService _contactService;
    private readonly IUserInterfaceServices _userInterfaceServices;

    public ViewContactDetailCommand(IContactService contactService, IUserInterfaceServices userInterfaceServices)
    {
        _contactService = contactService;
        _userInterfaceServices = userInterfaceServices;
    }

    public async Task ExecuteAsync()
    {
        bool viewContact = true;

        while (viewContact)
        {
            _userInterfaceServices.DisplayMenuTitle("Show Contact Details");

            var serviceResult = await _contactService.GetContactsFromListAsync();
            if (serviceResult.Status == ServiceStatus.SUCCESS && serviceResult.Result is List<IContact> contacts && contacts.Any())
            {
                var contactToView = _userInterfaceServices.GetUserSelectedContact(contacts, "\nEnter the email of the contact you like to view, or type 'abort' to return to the main menu: ");
                if (contactToView == null)
                {
                    break;
                }

                Console.Clear();
                Console.WriteLine($"\nName: {contactToView.FirstName} {contactToView.LastName}");
                Console.WriteLine($"Address: {contactToView.Address}");
                Console.WriteLine($"Email: {contactToView.Email}");
                Console.WriteLine($"Phone Number: {contactToView.PhoneNumber}");
            }
            else
            {
                _userInterfaceServices.ShowMessage("No contacts available to view.", isError: true);
                viewContact = false;
            }

            viewContact = _userInterfaceServices.AskToContinue("\nDo you want to view another contact?");
        }

        _userInterfaceServices.ReturnToMainMenu();
    }
}
