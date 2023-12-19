using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;

namespace ContactConsoleApplication.Commands;

public class DeleteContactCommand : ICommand
{
    private readonly IContactService _contactService;
    private readonly IUserInterfaceServices _userInterfaceServices;

    public DeleteContactCommand(IContactService contactService, IUserInterfaceServices userInterfaceServices)
    {
        _contactService = contactService;
        _userInterfaceServices = userInterfaceServices;
    }

    public void Execute()
    {
        bool deleteContacts = true;
        while (deleteContacts)
        {
            _userInterfaceServices.DisplayMenuTitle("Delete Contact");

            var serviceresult = _contactService.GetContactsFromList();
            if (serviceresult.Status == ServiceStatus.SUCCESS && serviceresult.Result is List<IContact> contacts && contacts.Any())
            {
                _userInterfaceServices.ShowContactList("Current Contacts", contacts);
                Console.WriteLine("\nType 'back' to return to the main menu.");

                Console.Write("\nEnter the Email of the Contact to Delete: ");
                var email = Console.ReadLine()!;

                if (string.IsNullOrEmpty(email))
                {
                    _userInterfaceServices.ShowMessage("\nEmail cannot be empty. Press any key to try again.", isError: true);
                    Console.ReadKey();
                    continue;
                }
                else if (email.Equals("back", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                else
                {
                    var getContact = _contactService.GetContactByEmailFromList(email);
                    if (getContact.Status == ServiceStatus.SUCCESS)
                    {
                        if (getContact.Result is IContact contact)
                        {
                            Console.Clear();
                            _userInterfaceServices.ShowContactDetails(contact, "Contact to Delete");

                            Console.Write("\nAre you sure you want to delete this contact? (Y/N): ");
                            var confirmation = Console.ReadLine()!;

                            if (confirmation.Trim().ToUpper() == "Y")
                            {
                                var deleteResult = _contactService.DeleteContact(email);
                                switch (deleteResult.Status)
                                {
                                    case ServiceStatus.DELETED:
                                        Console.Clear();
                                        _userInterfaceServices.ShowMessage("Contact deleted successfully.", isError: false);
                                        break;

                                    case ServiceStatus.FAILED:
                                        _userInterfaceServices.ShowMessage("An error occurred while deleting the contact.", isError: true);
                                        break;

                                    default:
                                        _userInterfaceServices.ShowMessage("Unexpected error occurred.", isError: true);
                                        break;
                                }
                            }
                            else
                            {
                                _userInterfaceServices.ShowMessage("\nContact deletion cancelled.", isError: true);
                            }
                        }
                    }
                    else if (getContact.Status == ServiceStatus.NOT_FOUND)
                    {
                        _userInterfaceServices.ShowMessage("Contact with that email address doesn´t exist.", isError: true);
                    }
                    else
                    {
                        _userInterfaceServices.ShowMessage("An error occurred while retrieving the contact.", isError: true);
                    }
                }
            }
            else
            {
                _userInterfaceServices.ShowMessage("There are no contacts in the list.", isError: true);
                break;
            }

            deleteContacts = _userInterfaceServices.AskToContinue("\nDo you want to delete another contact?");
        }

        _userInterfaceServices.ReturnToMainMenu();
    }
}
