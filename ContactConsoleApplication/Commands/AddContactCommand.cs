using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Models;

namespace ContactConsoleApplication.Commands;

public class AddContactCommand : ICommand
{
    private readonly IContactService _contactService;

    public AddContactCommand(IContactService contactService)
    {
        _contactService = contactService;
    }

    public void Execute()
    {
        bool addingContacts = true;

        while (addingContacts)
        {
            IContact contact = new Contact();

            DisplayMenuTitle("Add New Contact");

            contact.FirstName = ReadNonEmptyInput("First Name: ");
            contact.LastName = ReadNonEmptyInput("Last Name: ");
            contact.Address = ReadNonEmptyInput("Address: ");
            contact.Email = ReadNonEmptyInput("Email: ");
            contact.PhoneNumber = ReadNonEmptyInput("Phone Number: ");

            var serviceResult = _contactService.AddContact(contact);
            switch (serviceResult.Status)
            {
                case ServiceStatus.SUCCESS:
                    Console.Clear();
                    ShowMessage("The Contact was added successfully", isError: false);
                    ShowContactDetails(contact, "Added Contact");
                    break;

                case ServiceStatus.ALREADY_EXISTS:
                    ShowMessage("A Contact with this email already exists!", isError: true);
                    break;

                case ServiceStatus.FAILED:
                    ShowMessage("Failed when trying to add a contact to the contact list", isError: true);
                    break;
            }
            addingContacts = AskToContinue("\nDo you want to add another contact?");
        }
        ReturnToMainMenu();
    }
}
