using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactMauiApplication.Helpers;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using Contact = ContactServiceLibrary.Models.Contact;

namespace ContactMauiApplication.ViewModels;

public partial class UpdateContactViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private Contact? contact;

    private readonly IContactService _contactService;

    public UpdateContactViewModel(IContactService contactService)
    {
        _contactService = contactService;
    }

    [RelayCommand]
    private async Task UpdateContact()
    {
        if (Contact == null)
        {
            await ShowTemporaryMessageAsync("No contact selected for update.", Colors.Red);
            return;
        }

        string validationErrors = ContactValidator.ValidateContact(Contact, validateEmail: false);

        if (!string.IsNullOrEmpty(validationErrors))
        {
            await ShowTemporaryMessageAsync(validationErrors, Colors.Red);
            return;
        }

        var result = await _contactService.UpdateContactAsync(Contact);

        switch (result.Status)
        {
            case ServiceStatus.UPDATED:
                await ShowTemporaryMessageAsync("Contact successfully updated!", Colors.Green);
                await Shell.Current.GoToAsync("..");
                break;
            case ServiceStatus.NOT_FOUND:
                await ShowTemporaryMessageAsync("Contact not found.", Colors.Red);
                break;
            case ServiceStatus.FAILED:
                await ShowTemporaryMessageAsync("Failed to update contact. Please try again.", Colors.Red);
                break;
            default:
                await ShowTemporaryMessageAsync("An unexpected error occurred.", Colors.Red);
                break;
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Contact = (query["Contact"] as Contact)!;
    }
}
