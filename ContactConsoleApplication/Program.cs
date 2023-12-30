using ContactConsoleApplication.Commands;
using ContactConsoleApplication.Interfaces;
using ContactConsoleApplication.Services;
using ContactConsoleApplication.Utilities;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    var filePath = "c:/School/CSharp-Projects/Json/Console/contacts.json";

    services.AddSingleton<IFileService, FileService>();
    services.AddSingleton<IContactService, ContactService>();
    services.AddSingleton<IContactRepository>(provider => new ContactRepository(provider.GetRequiredService<IFileService>(), filePath));
    services.AddSingleton<IMenuService, MenuService>();
    services.AddSingleton<IUserInterfaceServices, UserInterfaceServices>();

    services.AddTransient<AddContactCommand>();
    services.AddTransient<DeleteContactCommand>();
    services.AddTransient<UpdateContactCommand>();
    services.AddTransient<ViewContactDetailCommand>();
    services.AddTransient<ViewContactListCommand>();

}).Build();

builder.Start();
Console.Clear();

var menuService = builder.Services.GetRequiredService<IMenuService>();
await menuService.ShowMainMenuAsync();
