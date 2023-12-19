using ContactConsoleApplication.Commands;
using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Interfaces;

namespace ContactConsoleApplication.Services;

public class MenuService : IMenuService
{
    private readonly IContactService _contactService;
    private readonly IUserInterfaceServices _userInterfaceService;

    public MenuService(IContactService contactService, IUserInterfaceServices userInterfaceService)
    {
        _contactService = contactService;
        _userInterfaceService = userInterfaceService;
    }

    public void ShowMainMenu()
    {
        while (true)
        {
            _userInterfaceService.DisplayMenuTitle("Menu Options");
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
                    var addContactCommand = new AddContactCommand(_contactService, _userInterfaceService);
                    addContactCommand.Execute();
                    break;
                case "2":
                    var deleteContactCommand = new DeleteContactCommand(_contactService, _userInterfaceService);
                    deleteContactCommand.Execute();
                    break;
                case "3":
                    var updateContactCommand = new UpdateContactCommand(_contactService, _userInterfaceService);
                    updateContactCommand.Execute();
                    break;
                case "4":
                    var viewContactDetailCommand = new ViewContactDetailCommand(_contactService, _userInterfaceService);
                    viewContactDetailCommand.Execute();
                    break;
                case "5":
                    var viewContactListCommand = new ViewContactListCommand(_contactService, _userInterfaceService);
                    viewContactListCommand.Execute();
                    break;
                case "0":
                    _userInterfaceService.ExitApplication();
                    break;
                default:
                    _userInterfaceService.ShowMessage("\nInvalid option Selected. Press any key to try again.", isError: true);
                    Console.ReadKey();
                    break;
            }   
        }
    }
}
