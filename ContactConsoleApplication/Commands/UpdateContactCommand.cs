using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;

namespace ContactConsoleApplication.Commands;

public class UpdateContactCommand : ICommand
{
    private readonly IContactService _contactService;
    private readonly IUserInterfaceServices _userInterfaceServices;

    public UpdateContactCommand(IContactService contactService, IUserInterfaceServices userInterfaceServices)
    {
        _contactService = contactService;
        _userInterfaceServices = userInterfaceServices;
    }

    public async Task ExecuteAsync()
    {
        bool updateContacts = true;
        while (updateContacts)
        {
            _userInterfaceServices.DisplayMenuTitle("Update Contact");

            var serviceResult = await _contactService.GetContactsFromListAsync();
            if (serviceResult.Status == ServiceStatus.SUCCESS && serviceResult.Result is List<IContact> contacts && contacts.Any())
            {
                var contactToUpdate = _userInterfaceServices.GetUserSelectedContact(contacts, "\nEnter the email of the contact to update, or type 'abort' to return to the main menu: ");
                if (contactToUpdate == null)
                {
                    break;
                }

                Console.Clear();
                _userInterfaceServices.ShowContactDetails(contactToUpdate, "Contact Details Before Update");
                Console.WriteLine("Note: The email address cannot be changed. To change the email address, delete and re-add the contact.");

                Console.Write("\nChange First Name (or press Enter to skip): ");
                var firstName = Console.ReadLine()!;
                if (!string.IsNullOrEmpty(firstName)) { contactToUpdate.FirstName = firstName; }

                Console.Write("Change Last Name (or press Enter to skip): ");
                var lastName = Console.ReadLine()!;
                if (!string.IsNullOrEmpty(lastName)) { contactToUpdate.LastName = lastName; }

                Console.Write("Change Address (or press Enter to skip): ");
                var address = Console.ReadLine()!;
                if (!string.IsNullOrEmpty(address)) { contactToUpdate.Address = address; }

                var phoneNumberInput = _userInterfaceServices.ReadValidPhoneNumber("Change Phone Number (or press Enter to skip): ", true);
                if (!string.IsNullOrEmpty(phoneNumberInput))
                {
                    contactToUpdate.PhoneNumber = phoneNumberInput;
                }

                try
                {
                    var updateResult = await _contactService.UpdateContactAsync(contactToUpdate);
                    switch (updateResult.Status)
                    {
                        case ServiceStatus.UPDATED:
                            Console.Clear();
                            _userInterfaceServices.ShowMessage("Contact updated successfully.", false);
                            _userInterfaceServices.ShowContactDetails(contactToUpdate, "Updated Contact Details");
                            break;

                        case ServiceStatus.FAILED:
                            _userInterfaceServices.ShowMessage($"Failed to update contact: {updateResult.Result}", true);
                            break;

                        default:
                            _userInterfaceServices.ShowMessage("Unexpected status when updating the contact.", true);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _userInterfaceServices.ShowMessage("An unexpected error occurred when updating the contact.", true, ex);
                }
            }
            else
            {
                _userInterfaceServices.ShowMessage("There are no contacts to update.", true);
                break;
            }

            updateContacts = _userInterfaceServices.AskToContinue("\nDo you want to update another contact?");
        }

        _userInterfaceServices.ReturnToMainMenu();
    }
}
