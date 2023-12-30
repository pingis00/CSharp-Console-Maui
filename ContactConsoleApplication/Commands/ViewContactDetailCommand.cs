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

            Console.Write("Enter the email of the contact you like to view: ");
            var email = Console.ReadLine();

            if (string.IsNullOrEmpty(email))
            {
                _userInterfaceServices.ShowMessage("Email cannot be empty.", isError: true);
                continue;
            }

            var serviceResult = await _contactService.GetContactByEmailFromListAsync(email);
            switch (serviceResult.Status)
            {
                case ServiceStatus.SUCCESS:
                    if (serviceResult.Result is IContact contact)
                    {
                        Console.Clear();
                        Console.WriteLine($"\nName: {contact.FirstName} {contact.LastName}");
                        Console.WriteLine($"Address: {contact.Address}");
                        Console.WriteLine($"Email: {contact.Email}");
                        Console.WriteLine($"Phone Number: {contact.PhoneNumber}");
                    }
                    break;

                case ServiceStatus.NOT_FOUND:
                    _userInterfaceServices.ShowMessage("Contact with that email doesn´t exist.", isError: true);
                    break;

                case ServiceStatus.FAILED:
                    _userInterfaceServices.ShowMessage("An error occurred while retrieving the contact.", isError: true);
                    break;
            }

            viewContact = _userInterfaceServices.AskToContinue("\nDo you want to view another contact?");
        }

        _userInterfaceServices.ReturnToMainMenu();
    }
}
