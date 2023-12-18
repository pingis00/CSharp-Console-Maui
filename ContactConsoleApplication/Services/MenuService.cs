using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Models;

namespace ContactConsoleApplication.Services;

public class MenuService : IMenuService
{
    private readonly IContactService _contactService;

    public MenuService(IContactService contactService)
    {
        _contactService = contactService;
    }

    public void ShowMainMenu()
    {
        while (true)
        {
            DisplayMenuTitle("Menu Options");
            Console.WriteLine($"{"1.", -3} Add new contact");
            Console.WriteLine($"{"2.", -3} Delete contact");
            Console.WriteLine($"{"3.", -3} Update contact");
            Console.WriteLine($"{"4.", -3} Show Contact Details");
            Console.WriteLine($"{"5.", -3} View Contact List");
            Console.WriteLine($"{"0.", -3} Exit Application");
            Console.Write("\nEnter Menu Option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowAddContactOption();
                    break;
                case "2":
                    ShowDeleteContactOption();
                    break;
                case "3":
                    ShowUpdateContactOption();
                    break;
                case "4":
                    ShowContactDetailOption();
                    break;
                case "5":
                    ShowViewContactsListOption();
                    break;
                case "0":
                    ShowExitApplicationOption();
                    break;
                default:
                    ShowMessage("\nInvalid option Selected. Press any key to try again.", isError: true);
                    Console.ReadKey();
                    break;
            }   
        }
    }

    private void ShowAddContactOption()
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

    private void ShowDeleteContactOption()
    {
        bool deleteContacts = true;
        while (deleteContacts)
        {
            DisplayMenuTitle("Delete Contact");
            
            var serviceresult = _contactService.GetContactsFromList();
            if (serviceresult.Status == ServiceStatus.SUCCESS && serviceresult.Result is List<IContact> contacts && contacts.Any())
            {
                ShowContactList("Current Contacts", contacts);
                Console.WriteLine("\nType 'back' to return to the main menu.");

                Console.Write("\nEnter the Email of the Contact to Delete: ");
                var email = Console.ReadLine()!;

                if (string.IsNullOrEmpty(email))
                {
                    ShowMessage("\nEmail cannot be empty. Press any key to try again.", isError: true);
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
                            ShowContactDetails(contact, "Contact to Delete");

                            Console.Write("\nAre you sure you want to delete this contact? (Y/N): ");
                            var confirmation = Console.ReadLine()!;

                            if (confirmation.Trim().ToUpper() == "Y")
                            {
                                var deleteResult = _contactService.DeleteContact(email);
                                switch (deleteResult.Status)
                                {
                                    case ServiceStatus.DELETED:
                                        Console.Clear();
                                        ShowMessage("Contact deleted successfully.", isError: false);
                                        break;

                                    case ServiceStatus.FAILED:
                                        ShowMessage("An error occurred while deleting the contact.", isError: true);
                                        break;

                                    default:
                                        ShowMessage("Unexpected error occurred.", isError: true);
                                        break;
                                }
                            }
                            else
                            {
                                ShowMessage("\nContact deletion cancelled.", isError: true);
                            }
                        }
                    }
                    else if (getContact.Status == ServiceStatus.NOT_FOUND)
                    {
                        ShowMessage("Contact with that email address doesn´t exist.", isError: true);
                    }
                    else
                    {
                        ShowMessage("An error occurred while retrieving the contact.", isError: true);
                    }
                }
            }
            else
            {
                ShowMessage("There are no contacts in the list.", isError: true);
                break;
            }

            deleteContacts = AskToContinue("\nDo you want to delete another contact?");
        }

        ReturnToMainMenu();
    }

    private void ShowUpdateContactOption()
    {
        bool updateContacts = true;
        while (updateContacts)
        {
            DisplayMenuTitle("Update Contact");

            var serviceResult = _contactService.GetContactsFromList();
            if (serviceResult.Status == ServiceStatus.SUCCESS && serviceResult.Result is List<IContact> contacts && contacts.Any())
            {
                ShowContactList("Current Contacts", contacts);
            }
            else
            {
                ShowMessage("There are no contacts to update.", isError: true);
                Console.ReadKey();
                return;
            }

            Console.Write("\nEnter the Email of the Contact to Update: ");
            var email = Console.ReadLine()!;

            if (string.IsNullOrEmpty(email))
            {
                ShowMessage("\nEmail cannot be empty. Press any key to try again.", isError: true);
                Console.ReadKey();
                continue;
            }

            var getContact = _contactService.GetContactByEmailFromList(email);

            switch (getContact.Status)
            {
                case ServiceStatus.SUCCESS:
                    var contactToUpdate = getContact.Result as IContact;
                    if (contactToUpdate != null)
                    {
                        Console.Clear();
                        ShowContactDetails(contactToUpdate, "Contact Details Before Update");
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

                        var updateResult = _contactService.UpdateContact(contactToUpdate);
                        switch (updateResult.Status)
                        {
                            case ServiceStatus.UPDATED:
                                Console.Clear();
                                ShowMessage("Contact updated successfully.", isError: false);
                                ShowContactDetails(contactToUpdate, "Updated Contact Details");
                                break;

                            case ServiceStatus.FAILED:
                                ShowMessage("An error occurred while updating the contact.", isError: true);
                                break;

                            default:
                                ShowMessage("Unexpected status when updating the contact.", isError: true);
                                break;
                        }
                    }
                    break;

                case ServiceStatus.NOT_FOUND:
                    ShowMessage("\nContact with that email doesn´t exist.", isError: true);
                    break;

                case ServiceStatus.FAILED:
                    ShowMessage("\nAn error occurred while retrieving the contact.", isError: true);
                    break;

                default:
                    ShowMessage("\nUnexpected status when retrieving the contact.", isError: true);
                    break;
            }

            updateContacts = AskToContinue("\nDo you want to update another contact?");
        }
        ReturnToMainMenu();
    }

    private void ShowContactDetailOption()
    {
        bool viewContact = true;

        while (viewContact)
        {
            DisplayMenuTitle("Show Contact Details");

            Console.Write("Enter the email of the contact you like to view: ");
            var email = Console.ReadLine();

            if (string.IsNullOrEmpty(email))
            {
                ShowMessage("Email cannot be empty.", isError: true);
                continue;
            }

            var serviceResult = _contactService.GetContactByEmailFromList(email);
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
                    ShowMessage("Contact with that email doesn´t exist.", isError: true);
                    break;

                case ServiceStatus.FAILED:
                    ShowMessage("An error occurred while retrieving the contact.", isError: true);
                    break;
            }

            viewContact = AskToContinue("\nDo you want to view another contact?");
        }

        ReturnToMainMenu();
    }

    private void ShowViewContactsListOption()
    {
        DisplayMenuTitle("Show Contact List");

        Console.WriteLine("Sort by: \n1. First Name \n2. Last Name \n3. Email \nPress any other key for unsorted");
        Console.Write("\nChoose an option: ");
        var sortOption = Console.ReadLine();

        var serviceResult = _contactService.GetContactsFromList();

        if (serviceResult.Status == ServiceStatus.SUCCESS)
        {
            if (serviceResult.Result is List<IContact> contacts && contacts.Any())
            {
                string sortMethod;
                var sortedContacts = SortContacts(contacts, sortOption!, out sortMethod);
                ShowContactList("Contact List", sortedContacts, sortMethod);
            }
            else
            {
                ShowMessage("There are no contacts in the list.", isError: true);
            }
        }
        else
        {
            ShowMessage("An error occurred while retrieving the contact list.", isError: true);
        }

        ReturnToMainMenu();
    }

    private void ShowExitApplicationOption()
    {
        Console.Clear();
        Console.Write("Are you sure you want to close this application? (y/n): ");
        var option = Console.ReadLine() ?? "";

        if (option.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            ShowMessage("Application exited. Press any key to continue", isError: false);
            Console.ReadKey();
            Environment.Exit(0);
        }
        else
        {
            ShowMessage("Exiting cancelled. Returning to main menu by pressing any key.", isError: false);
            Console.ReadKey();
        }
    }

    private void DisplayMenuTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"## {title} ##");
        Console.WriteLine();
    }

    private void ReturnToMainMenu()
    {
        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey();
    }

    private void ShowContactDetails(IContact contact, string title)
    {
        Console.WriteLine($"\n{title}:");
        Console.WriteLine(new string('-', 40));
        Console.WriteLine($"{"First Name:", -15} {contact.FirstName}");
        Console.WriteLine($"{"Last Name:", -15} {contact.LastName}");
        Console.WriteLine($"{"Address:", -15} {contact.Address}");
        Console.WriteLine($"{"Email:", -15} {contact.Email}");
        Console.WriteLine($"{"Phone Number:", -15} {contact.PhoneNumber}");
        Console.WriteLine(new string('-', 40));
    }

    private string ReadNonEmptyInput(string prompt)
    {
        string input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine()!;

            if (string.IsNullOrEmpty(input))
            {
                ShowMessage("\nThis field cannot be empty. Please enter a value.", isError: true);
                Console.WriteLine("\nPress any key to try again...");
                Console.ReadKey();
                Console.Clear();
                DisplayMenuTitle("Add New Contact");
            }
        } 
        while (string.IsNullOrEmpty(input));
        return input;
    }

    private void ShowContactList(string title, List<IContact> contacts, string sortMethod = "")
    {
        Console.Clear();
        DisplayMenuTitle(title);

        if (!string.IsNullOrEmpty(sortMethod))
        {
            Console.WriteLine($"List sorted by: {sortMethod}");
            Console.WriteLine(new string('-', 40));
        }

        Console.WriteLine($"\n{"First Name",-15} {"Last Name",-15} {"Address",-20} {"Email",-25} {"Phone",-15}");
        Console.WriteLine(new string('-', 90));
        foreach (var contact in contacts)
        {
            Console.WriteLine($"{contact.FirstName,-15} {contact.LastName,-15} {contact.Address,-20} {contact.Email,-25} {contact.PhoneNumber,-15}");
        }
    }

    private bool AskToContinue(string message)
    {
        Console.Write($"{message} (Y/N): ");
        var choice = Console.ReadLine();
        return choice.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase);
    }

    private void ShowMessage(string message, bool isError = false)
    {
        if (isError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor= ConsoleColor.Green;
        }
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private List<IContact> SortContacts(List<IContact> contacts, string sortOption, out string sortMethod)
    {
        sortMethod = sortOption switch
        {
            "1" => "First Name",
            "2" => "Last Name",
            "3" => "Email",
            _ => "Unsorted",
        };

        return sortOption switch
        {
            "1" => contacts.OrderBy(c => c.FirstName).ToList(),
            "2" => contacts.OrderBy(c => c.LastName).ToList(),
            "3" => contacts.OrderBy(c => c.Email).ToList(),
            _ => contacts,
        };
    }
}
