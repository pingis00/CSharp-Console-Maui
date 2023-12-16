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
                    Console.WriteLine("\nInvalid option Selected. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
            
        }
    }

    private void ShowAddContactOption()
    {
        bool contactAddedOrAborted = false;

        while (!contactAddedOrAborted)
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
                    Console.WriteLine("The Contact was added successfully");
                    ShowContactDetails(contact, "Added Contact");
                    
                    if (!contactAddedOrAborted)
                    {
                        Console.Write("\nDo you want to add another contact? (Y/N): ");
                        var choice = Console.ReadLine();
                        if (choice.Trim().ToUpper() == "Y")
                        {
                            Console.Clear();
                            contactAddedOrAborted = false;
                        }
                        else
                        {
                            contactAddedOrAborted = true;
                        }
                    }
                    break;

                case ServiceStatus.ALREADY_EXISTS:
                    Console.WriteLine(new string('-', 40));
                    Console.WriteLine("\nA Contact with this email-address already exists!");
                    Console.Write("\nWould you like to try a different email? (Y/N): ");
                    if (Console.ReadLine()!.Trim().ToUpper() != "Y")
                    {
                        contactAddedOrAborted = true;
                    }
                    break;

                case ServiceStatus.FAILED:
                    Console.WriteLine("Failed when trying to add a contact to the contact list");
                    contactAddedOrAborted = true;
                    break;
            }
            if (contactAddedOrAborted)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private void ShowDeleteContactOption()
    {
        bool continueDeleting = true;
        while (continueDeleting)
        {
            Console.Clear();
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
                    Console.WriteLine("\nEmail cannot be empty. Press any key to try again.");
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
                                        Console.WriteLine("Contact deleted successfully.");
                                        break;

                                    case ServiceStatus.FAILED:
                                        Console.WriteLine("An error occurred while deleting the contact.");
                                        break;

                                    default:
                                        Console.WriteLine("Unexpected error occurred.");
                                        break;
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Contact deletion cancelled.");
                            }
                        }
                    }
                    else if (getContact.Status == ServiceStatus.NOT_FOUND)
                    {
                        Console.WriteLine("Contact with that email address doesn´t exist.");
                    }
                    else
                    {
                        Console.WriteLine("An error occurred while retrieving the contact.");
                    }
                }
            }
            else
            {
                Console.WriteLine("There are no contacts in the list.");
                continueDeleting = false;
            }

            if (continueDeleting)
            {
                Console.Write("\nDo you want to delete another contact? (Y/N): ");
                var choice = Console.ReadLine();
                if (choice.Trim().ToUpper() == "Y")
                {
                    Console.Clear();
                    continueDeleting = true;
                }
                else
                {
                    continueDeleting = false;
                }
            }
        }

        Console.WriteLine("\nPress any key to return to the main menu");
        Console.ReadKey();
    }

    private void ShowUpdateContactOption()
    {
        DisplayMenuTitle("Update Contact");

        Console.Write("Enter the Email of the Contact to Update: ");
        var email = Console.ReadLine()!;

        if (string.IsNullOrEmpty(email))
        {
            Console.WriteLine("Email cannot be empty.");
            Console.ReadKey();
            return;
        }

        var getContact = _contactService.GetContactByEmailFromList(email);

        switch (getContact.Status)
        {
            case ServiceStatus.SUCCESS:
                var contactToUpdate = getContact.Result as IContact;
                if (contactToUpdate != null)
                {

                    Console.Write($"{contactToUpdate.FirstName}: Change First name (or press Enter to skip): ");
                    var firstName = Console.ReadLine()!;
                    if (!string.IsNullOrEmpty(firstName)) { contactToUpdate.FirstName = firstName; }

                    Console.Write($"{contactToUpdate.LastName}: Change Last name (or press Enter to skip): ");
                    var lastName = Console.ReadLine()!;
                    if (!string.IsNullOrEmpty(lastName)) { contactToUpdate.LastName = lastName; }

                    Console.Write($"{contactToUpdate.Address}: Change Address (or press Enter to skip): ");
                    var Address = Console.ReadLine()!;
                    if (!string.IsNullOrEmpty(Address)) { contactToUpdate.Address = Address; }

                    Console.Write($"{contactToUpdate.Email}: Change Email (or press Enter to skip): ");
                    var Email = Console.ReadLine()!;
                    if (!string.IsNullOrEmpty(Email)) { contactToUpdate.Email = Email; }

                    Console.Write($"{contactToUpdate.PhoneNumber}: Change phone number (or press Enter to skip): ");
                    var phoneNumber = Console.ReadLine()!;
                    if (!string.IsNullOrEmpty(phoneNumber)) { contactToUpdate.PhoneNumber = phoneNumber; }

                    var updateResult = _contactService.UpdateContact(contactToUpdate);
                    switch (updateResult.Status)
                    {
                        case ServiceStatus.UPDATED:
                            Console.Clear();
                            Console.WriteLine("Contact updated successfully.");
                            ShowContactDetails(contactToUpdate, "Updated Contact Details");
                            break;

                        case ServiceStatus.FAILED:
                            Console.WriteLine("An error occurred while updating the contact.");
                            break;

                        default:
                            Console.WriteLine("Unexpected status when updating the contact.");
                            break;
                    }
                }
                break;

            case ServiceStatus.NOT_FOUND:
                Console.WriteLine("Contact with that email doesn´t exist.");
                break;

            case ServiceStatus.FAILED:
                Console.WriteLine("An error occurred while retrieving the contact.");
                break;

            default:
                Console.WriteLine("Unexpected status when retrieving the contact.");
                break;
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void ShowContactDetailOption()
    {
        DisplayMenuTitle("Show Contact Details");

        Console.Write("Enter the email of the contact you like to view: ");
        var email = Console.ReadLine();

        if (string.IsNullOrEmpty(email))
        {
            Console.WriteLine("Email cannot be empty.");
            return;
        }

        var serviceResult = _contactService.GetContactByEmailFromList(email);
        switch (serviceResult.Status)
        {
            case ServiceStatus.SUCCESS:
                if (serviceResult.Result is IContact contact)
                {
                    Console.WriteLine($"Name: {contact.FirstName} {contact.LastName}");
                    Console.WriteLine($"Address: {contact.Address}");
                    Console.WriteLine($"Email: {contact.Email}");
                    Console.WriteLine($"Phone Number: {contact.PhoneNumber}");
                }             
                break;
            case ServiceStatus.NOT_FOUND:
                Console.WriteLine("Contact with that email doesn´t exist");
                break;
            case ServiceStatus.FAILED:
                Console.WriteLine("An error occurred while retrieving the contact.");
                break;
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void ShowViewContactsListOption()
    {
        DisplayMenuTitle("Show Contact List");

        Console.WriteLine("Sort by: \n1. First Name \n2. Last Name \n3. Email");
        Console.Write("Choose an option: ");
        var sortOption = Console.ReadLine();

        var serviceResult = _contactService.GetContactsFromList();

        if (serviceResult.Status == ServiceStatus.SUCCESS)
        {
            if (serviceResult.Result is List<IContact> contacts && contacts.Any())
            {
                switch (sortOption)
                {
                    case "1":
                        contacts = contacts.OrderBy(c => c.FirstName).ToList();
                        break;

                    case "2":
                        contacts = contacts.OrderBy(c => c.LastName).ToList();
                        break;

                    case "3":
                        contacts = contacts.OrderBy(c => c.Email).ToList();
                        break;
                }

                Console.Clear();
                Console.WriteLine($"{"First Name",-15} {"Last Name",-15} {"Address",-20} {"Email",-25} {"Phone",-15}");
                Console.WriteLine(new string('-', 90));
                foreach (var contact in contacts)
                {
                    Console.WriteLine($"{contact.FirstName,-15} {contact.LastName,-15} {contact.Address,-20} {contact.Email,-25} {contact.PhoneNumber,-15}");
                }
            }
            else
            {
                Console.WriteLine("There are no contacts in the list.");
            }
        }
        else
        {
            Console.WriteLine("An error occurred while retrieving the contact list.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void ShowExitApplicationOption()
    {
        Console.Clear();
        Console.Write("Are you sure you want to close this application? (y/n): ");
        var option = Console.ReadLine() ?? "";

        if (option.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            Environment.Exit(0);
        }
    }

    private void DisplayMenuTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"## {title} ##");
        Console.WriteLine();
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
                Console.WriteLine("\nThis field cannot be empty. Please enter a value.");
                Console.WriteLine("\nPress any key to try again...");
                Console.ReadKey();
                Console.Clear();
                DisplayMenuTitle("Add New Contact");
            }
        } 
        while (string.IsNullOrEmpty(input));
        return input;
    }

    private void ShowContactList(string title, List<IContact> contacts)
    {
        Console.Clear();
        DisplayMenuTitle(title);
        Console.WriteLine($"{"First Name",-15} {"Last Name",-15} {"Address",-20} {"Email",-25} {"Phone",-15}");
        Console.WriteLine(new string('-', 90));
        foreach (var contact in contacts)
        {
            Console.WriteLine($"{contact.FirstName,-15} {contact.LastName,-15} {contact.Address,-20} {contact.Email,-25} {contact.PhoneNumber,-15}");
        }
    }

}
