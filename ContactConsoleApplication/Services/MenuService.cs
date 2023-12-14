using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Models;

namespace ContactConsoleApplication.Services;

public class MenuService : IMenuService
{
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
        IContact contact = new Contact();

        DisplayMenuTitle("Add New Contact");
        Console.Write("First Name: ");
        contact.FirstName = Console.ReadLine()!;

        Console.Write("Last Name: ");
        contact.LastName = Console.ReadLine()!;

        Console.Write("Address: ");
        contact.Address = Console.ReadLine()!;

        Console.Write("Email: ");
        contact.Email = Console.ReadLine()!;

        Console.Write("Phone Number: ");
        contact.PhoneNumber = Console.ReadLine()!;
    }

    private void ShowContactDetailOption()
    {
        throw new NotImplementedException();
    }

    private void ShowDeleteContactOption()
    {
        throw new NotImplementedException();
    }

    private void ShowUpdateContactOption()
    {
        throw new NotImplementedException();
    }

    private void ShowViewContactsListOption()
    {
        throw new NotImplementedException();
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
}
