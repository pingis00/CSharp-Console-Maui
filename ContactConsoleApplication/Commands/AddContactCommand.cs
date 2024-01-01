using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Models;

namespace ContactConsoleApplication.Commands;

public class AddContactCommand : ICommand
{
    private readonly IContactService _contactService;
    private readonly IUserInterfaceServices _userInterfaceServices;

    public AddContactCommand(IContactService contactService, IUserInterfaceServices userInterfaceServices)
    {
        _contactService = contactService;
        _userInterfaceServices = userInterfaceServices;
    }

    public async Task ExecuteAsync()
    {
        bool addingContacts = true;

        while (addingContacts)
        {
            IContact contact = new Contact();

            _userInterfaceServices.DisplayMenuTitle("Add New Contact");

            contact.FirstName = _userInterfaceServices.ReadNonEmptyInput("First Name: ");
            contact.LastName = _userInterfaceServices.ReadNonEmptyInput("Last Name: ");
            contact.Address = _userInterfaceServices.ReadNonEmptyInput("Address: ");
            contact.Email = _userInterfaceServices.ReadValidEmail("Email: ");
            contact.PhoneNumber = _userInterfaceServices.ReadValidPhoneNumber("Phone Number: ");

            Console.Clear();
            _userInterfaceServices.ShowContactDetails(contact, "Contact to add");
            bool confirmAdd = _userInterfaceServices.AskToContinue("Do you want to save this contact?");

            if (confirmAdd)
            {
                var serviceResult = await _contactService.AddContactAsync(contact);
                switch (serviceResult.Status)
                {
                    case ServiceStatus.SUCCESS:
                        Console.Clear();
                        _userInterfaceServices.ShowMessage("The Contact was added successfully", isError: false);
                        break;

                    case ServiceStatus.ALREADY_EXISTS:
                        _userInterfaceServices.ShowMessage("A Contact with this email already exists!", isError: true);
                        break;

                    case ServiceStatus.FAILED:
                        _userInterfaceServices.ShowMessage($"Failed when trying to add a contact to the contact list!", isError: true);
                        break;
                }
            }
            else
            {
                Console.Clear();
                _userInterfaceServices.ShowMessage("Contact not saved.", isError: true);
            }
            addingContacts = _userInterfaceServices.AskToContinue("\nDo you want to add another contact?");
        }

        _userInterfaceServices.ReturnToMainMenu();
    }
}
