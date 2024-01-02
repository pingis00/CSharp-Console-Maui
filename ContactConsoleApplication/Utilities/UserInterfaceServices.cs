using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Utilities;

namespace ContactConsoleApplication.Utilities;

public class UserInterfaceServices : IUserInterfaceServices
{
    public bool AskToContinue(string message)
    {
        string choice;
        do
        {
            Console.Write($"{message} (Y/N): ");
            choice = Console.ReadLine()!.Trim().ToUpper();

            if (choice != "Y" && choice != "N")
            {
                Console.Clear();
                ShowMessage("Invalid input. Please enter 'Y' for Yes or 'N' for No.", true);
            }
        } while (choice != "Y" && choice != "N");

        return choice == "Y";
    }

    public void DisplayMenuTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"## {title} ##\n");
    }

    public string ReadNonEmptyInput(string prompt)
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

    public void ReturnToMainMenu()
    {
        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey();
    }

    public void ShowContactDetails(IContact contact, string title)
    {
        Console.WriteLine($"\n{title}:");
        Console.WriteLine(new string('-', 40));
        Console.WriteLine($"{"First Name:",-15} {contact.FirstName}");
        Console.WriteLine($"{"Last Name:",-15} {contact.LastName}");
        Console.WriteLine($"{"Address:",-15} {contact.Address}");
        Console.WriteLine($"{"Email:",-15} {contact.Email}");
        Console.WriteLine($"{"Phone Number:",-15} {contact.PhoneNumber}");
        Console.WriteLine(new string('-', 40));
    }

    public void ShowContactList(string title, List<IContact> contacts, string? sortOption = "")
    {
        Console.Clear();
        DisplayMenuTitle(title);

        IEnumerable<IContact> sortedContacts = sortOption switch
        {
            "1" => contacts.OrderBy(c => c.FirstName),
            "2" => contacts.OrderBy(c => c.LastName),
            "3" => contacts.OrderBy(c => c.Email),
            _ => contacts
        };

        var sortMethod = sortOption switch
        {
            "1" => "First Name",
            "2" => "Last Name",
            "3" => "Email",
            _ => "Unsorted"
        };
        
        Console.WriteLine($"List sorted by: {sortMethod}");
        Console.WriteLine(new string('-', 90));
        Console.WriteLine($"\n{"First Name",-15} {"Last Name",-15} {"Address",-20} {"Email",-25} {"Phone",-15}");
        Console.WriteLine(new string('-', 90));

        foreach (var contact in sortedContacts)
        {
            Console.WriteLine($"{contact.FirstName,-15} {contact.LastName,-15} {contact.Address,-20} {contact.Email,-25} {contact.PhoneNumber,-15}");
        }
    }

    public void ExitApplication()
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

    public void ShowMessage(string message, bool isError)
    {
        if (isError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public string ReadValidEmail(string prompt)
    {
        string input;
        while (true)
        {
            Console.Write(prompt);
            input = Console.ReadLine()!;

            if (string.IsNullOrEmpty(input))
            {
                Console.Clear();
                ShowMessage("Email address cannot be empty.\n", isError: true);
                continue;
            }

            if (!ValidationUtility.IsValidEmail(input))
            {
                Console.Clear();
                ShowMessage("Invalid email format. Expected format: example@domain.com.\n", isError: true);
                continue;
            }

            break;
        }

        return input;
    }

    public string ReadValidPhoneNumber(string prompt, bool allowEmpty = false)
    {
        string input;
        while (true)
        {
            Console.Write(prompt);
            input = Console.ReadLine()!;

            if (string.IsNullOrEmpty(input))
            {
                if (allowEmpty) return "";
                Console.Clear();
                ShowMessage("Phone number cannot be empty.\n", isError: true);
            }
            else if (!ValidationUtility.IsValidPhoneNumber(input))
            {
                Console.Clear();
                ShowMessage("Invalid phone number format. Expected format: +1234567890 or 0123456789 without spaces or hyphens.\n", isError: true);
            }
            else
            {
                break;
            }
        }

        return input;
    }

    public IContact GetUserSelectedContact(List<IContact> contacts, string prompt)
    {
        IContact? selectedContact = null!;
        while (selectedContact == null)
        {
            ShowContactList("Available Contacts", contacts);
            Console.WriteLine(prompt);
            var email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowMessage("Email cannot be empty. Press Enter to try again.", isError: true);
                Console.ReadKey();
                continue;
            }

            if (email.Equals("abort", StringComparison.OrdinalIgnoreCase))
            {
                return null!;
            }

            selectedContact = contacts.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (selectedContact == null)
            {
                ShowMessage("\nNo contact found with the specified email. Press Enter to try again.", isError: true);
                Console.ReadKey();
            }
        }

        return selectedContact;
    }

    public void ShowMessage(string message, bool isError, Exception? ex = null)
    {
        Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();

        if (ex != null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Exception: {ex.Message}");
            Console.ResetColor();
        }
    }
}
