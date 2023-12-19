﻿using ContactConsoleApplication.Commands;
using ContactConsoleApplication.Interfaces;
using ContactServiceLibrary.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ContactConsoleApplication.Services;

public class MenuService : IMenuService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserInterfaceServices _userInterfaceService;

    public MenuService(IServiceProvider serviceProvider, IUserInterfaceServices userInterfaceService)
    {
        _serviceProvider = serviceProvider;
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
                    var addContactCommand = _serviceProvider.GetRequiredService<AddContactCommand>();
                    addContactCommand.Execute();
                    break;
                case "2":
                    var deleteContactCommand = _serviceProvider.GetRequiredService<DeleteContactCommand>();
                    deleteContactCommand.Execute();
                    break;
                case "3":
                    var updateContactCommand = _serviceProvider.GetRequiredService<UpdateContactCommand>();
                    updateContactCommand.Execute();
                    break;
                case "4":
                    var viewContactDetailCommand = _serviceProvider.GetRequiredService<ViewContactDetailCommand>();
                    viewContactDetailCommand.Execute();
                    break;
                case "5":
                    var viewContactListCommand = _serviceProvider.GetRequiredService<ViewContactListCommand>();
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
