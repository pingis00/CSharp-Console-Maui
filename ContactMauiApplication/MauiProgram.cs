using ContactMauiApplication.Pages;
using ContactMauiApplication.ViewModels;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Repositories;
using Microsoft.Extensions.Logging;

namespace ContactMauiApplication
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<HomePageViewModel>();
            builder.Services.AddSingleton<AddContactViewModel>();
            builder.Services.AddSingleton<ViewContactListViewModel>();

            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<AddContactPage>();
            builder.Services.AddSingleton<ViewContactListPage>();
            builder.Services.AddSingleton<DeleteContactPage>();

            builder.Services.AddSingleton<IFileService, FileService>();

            string contactsFilePath = "c:/School/CSharp-Projects/Json/Maui/contacts.json";
            builder.Services.AddSingleton<IContactRepository>(serviceProvider =>
            {
                var fileServicce = serviceProvider.GetRequiredService<IFileService>();
                return new ContactRepository(fileServicce, contactsFilePath);
            });

            builder.Services.AddSingleton<IContactService, ContactService>();

            builder.Logging.AddDebug();

            return builder.Build();
        }
    }
}
