using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using Contact = ContactServiceLibrary.Models.Contact;

namespace ContactMauiApplication.ViewModels;

public partial class DeleteContactViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    private string? message;

    [ObservableProperty]
    private Color messageColor = Colors.Transparent;

    [ObservableProperty]
    private bool isMessageVisible;

    [ObservableProperty]
    private Contact? contact;

    private readonly IContactService _contactService;

    public DeleteContactViewModel(IContactService contactService)
    {
        _contactService = contactService;
    }



    [RelayCommand]
    private async Task DeleteContact()
    {
        if (Contact != null)
        {
            var result = await _contactService.DeleteContactAsync(Contact.Email);

            if (result.Status == ServiceStatus.DELETED)
            {
                Contact = null;
                await ShowTemporaryMessageAsync("Contact successfully deleted!", Colors.Green);
                await Shell.Current.GoToAsync("..");
                
            }
            else
            {
                await ShowTemporaryMessageAsync("Failed to delete contact. Please try again.", Colors.Red);
            }
        }
    }

    private async Task ShowTemporaryMessageAsync(string message, Color color)
    {
        Message = message;
        MessageColor = color;
        IsMessageVisible = true;
        await Task.Delay(3000);
        IsMessageVisible = false;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Contact = (query["Contact"] as Contact)!;
    }
}
