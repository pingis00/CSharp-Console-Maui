using ContactConsoleApplication.Interfaces;
using ContactConsoleApplication.Services;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddSingleton<IFileService, FileService>();
    services.AddSingleton<IContactService, ContactService>();
    services.AddSingleton<IContactRepository, ContactRepository>();
    services.AddSingleton<IMenuService, MenuService>();

}).Build();

builder.Start();
Console.Clear();

var menuService = builder.Services.GetRequiredService<IMenuService>();
menuService.ShowMainMenu();
