using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Interfaces;

namespace ContactConsoleApplication.Utilities;

public class UserInterfaceServices : IUserInterfaceServices
{
    public bool AskToContinue(string message)
    {
        Console.Write($"{message} (Y/N): ");
        var choice = Console.ReadLine();
        return choice.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase);
    }

    public void DisplayMenuTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"## {title} ##");
        Console.WriteLine();
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

    public void ShowContactList(string title, List<IContact> contacts, string sortMethod = "")
    {
        Console.Clear();
        DisplayMenuTitle(title);

        if (!string.IsNullOrEmpty(sortMethod))
        {
            Console.WriteLine($"List sorted by: {sortMethod}");
            Console.WriteLine(new string('-', 90));
        }

        Console.WriteLine($"\n{"First Name",-15} {"Last Name",-15} {"Address",-20} {"Email",-25} {"Phone",-15}");
        Console.WriteLine(new string('-', 90));
        foreach (var contact in contacts)
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

    public (List<IContact> SortedContacts, string SortMethod) SortContacts(List<IContact> contacts, string sortOption)
    {
        var sortMethod = sortOption switch
        {
            "1" => "First Name",
            "2" => "Last Name",
            "3" => "Email",
            _ => "Unsorted",
        };

        var sortedContacts = sortOption switch
        {
            "1" => contacts.OrderBy(c => c.FirstName).ToList(),
            "2" => contacts.OrderBy(c => c.LastName).ToList(),
            "3" => contacts.OrderBy(c => c.Email).ToList(),
            _ => contacts,
        };

        return (sortedContacts, sortMethod);
    }
}