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

    public async Task ExecuteAsync()
    {
        bool deleteContacts = true;
        while (deleteContacts)
        {
            _userInterfaceServices.DisplayMenuTitle("Delete Contact");

            var serviceResult = await _contactService.GetContactsFromListAsync();
            if (serviceResult.Status == ServiceStatus.SUCCESS && serviceResult.Result is List<IContact> contacts && contacts.Any())
            {
                var contactToDelete = _userInterfaceServices.GetUserSelectedContact(contacts, "\nEnter the email of the contact to delete, or type 'abort' to return to the main menu: ");
                if (contactToDelete == null)
                {
                    break;
                }

                Console.Clear();
                _userInterfaceServices.ShowContactDetails(contactToDelete, "Contact to Delete");
                bool confirmDelete = _userInterfaceServices.AskToContinue("\nAre you sure you want to delete this contact?");

                try
                {
                    if (confirmDelete)
                    {
                        var deleteResult = await _contactService.DeleteContactAsync(contactToDelete.Email);
                        switch (deleteResult.Status)
                        {
                            case ServiceStatus.DELETED:
                                Console.Clear();
                                _userInterfaceServices.ShowMessage("Contact deleted successfully.", false);
                                break;
                            case ServiceStatus.FAILED:
                                _userInterfaceServices.ShowMessage("An error occurred while deleting the contact.", true);
                                break;
                            default:
                                _userInterfaceServices.ShowMessage("Unexpected error occurred.", true);
                                break;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        _userInterfaceServices.ShowMessage("\nContact deletion cancelled.", true);
                    }
                }
                catch (Exception ex)
                {
                    _userInterfaceServices.ShowMessage("An unexpected error occurred during contact deletion.", true, ex);
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
