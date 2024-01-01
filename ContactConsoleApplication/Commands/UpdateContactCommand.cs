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
                _userInterfaceServices.ShowContactList("Current Contacts", contacts);
            }
            else
            {
                _userInterfaceServices.ShowMessage("There are no contacts to update.", isError: true);
                Console.ReadKey();
                return;
            }

            Console.Write("\nEnter the Email of the Contact to Update: ");
            var email = Console.ReadLine()!;

            if (string.IsNullOrEmpty(email))
            {
                _userInterfaceServices.ShowMessage("\nEmail cannot be empty. Press any key to try again.", isError: true);
                Console.ReadKey();
                continue;
            }

            var getContact = await _contactService.GetContactByEmailFromListAsync(email);

            switch (getContact.Status)
            {
                case ServiceStatus.SUCCESS:
                    var contactToUpdate = getContact.Result as IContact;
                    if (contactToUpdate != null)
                    {
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
                        var Address = Console.ReadLine()!;
                        if (!string.IsNullOrEmpty(Address)) { contactToUpdate.Address = Address; }

                        Console.Write("Change Phone Number (or press Enter to skip): ");
                        var phoneNumber = Console.ReadLine()!;
                        if (!string.IsNullOrEmpty(phoneNumber)) { contactToUpdate.PhoneNumber = phoneNumber; }

                        var updateResult = await _contactService.UpdateContactAsync(contactToUpdate);
                        switch (updateResult.Status)
                        {
                            case ServiceStatus.UPDATED:
                                Console.Clear();
                                _userInterfaceServices.ShowMessage("Contact updated successfully.", isError: false);
                                _userInterfaceServices.ShowContactDetails(contactToUpdate, "Updated Contact Details");
                                break;

                            case ServiceStatus.FAILED:
                                _userInterfaceServices.ShowMessage($"Failed to update contact: {updateResult.Result}", isError: true);
                                break;

                            default:
                                _userInterfaceServices.ShowMessage("Unexpected status when updating the contact.", isError: true);
                                break;
                        }
                    }
                    break;

                case ServiceStatus.NOT_FOUND:
                    _userInterfaceServices.ShowMessage("\nContact with that email doesn´t exist.", isError: true);
                    break;

                case ServiceStatus.FAILED:
                    _userInterfaceServices.ShowMessage("\nAn error occurred while retrieving the contact.", isError: true);
                    break;

                default:
                    _userInterfaceServices.ShowMessage("\nUnexpected status when retrieving the contact.", isError: true);
                    break;
            }

            updateContacts = _userInterfaceServices.AskToContinue("\nDo you want to update another contact?");
        }
        _userInterfaceServices.ReturnToMainMenu();
    }
}
