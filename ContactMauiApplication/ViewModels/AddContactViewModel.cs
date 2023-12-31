using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactMauiApplication.Helpers;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using System.Windows.Input;
using Contact = ContactServiceLibrary.Models.Contact;

namespace ContactMauiApplication.ViewModels;

public partial class AddContactViewModel : ObservableObject
{
    [ObservableProperty]
    private string? firstName;

    [ObservableProperty]
    private string? lastName;

    [ObservableProperty]
    private string? address;

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? phoneNumber;

    [ObservableProperty]
    private string? message;

    [ObservableProperty]
    private Color messageColor = Colors.Transparent;

    [ObservableProperty]
    private bool isMessageVisible;

    private readonly IContactService _contactService;

    public ICommand AddContactCommand { get; }

    public AddContactViewModel(IContactService contactService)
    {
        _contactService = contactService;

        AddContactCommand = new RelayCommand(async () => await AddContactAsync());
    }

    public async Task AddContactAsync()
    {
        var newContact = new Contact
        {
            FirstName = FirstName!,
            LastName = LastName!,
            Address = Address!,
            Email = Email!,
            PhoneNumber = PhoneNumber!,
        };

        var validationErrors = ContactValidator.ValidateContact(newContact);

        if (!string.IsNullOrEmpty(validationErrors))
        {
            await ShowTemporaryMessageAsync(validationErrors, Colors.Red);
            return;
        }

        var result = await _contactService.AddContactAsync(newContact);

        switch (result.Status)
        {
            case ServiceStatus.SUCCESS:
                ResetForm();
                await ShowTemporaryMessageAsync("Contact successfully added!", Colors.Green); 
                break;

            case ServiceStatus.ALREADY_EXISTS:
                await ShowTemporaryMessageAsync("A contact with this email already exists.", Colors.Red);
                break;

            case ServiceStatus.FAILED:
                await ShowTemporaryMessageAsync("Failed to add contact. Please try again.", Colors.Red);
                break;

            default:
                await ShowTemporaryMessageAsync("An unexpected error occurred.", Colors.Red);
                break;
        }
    }

    private void ResetForm()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Address = string.Empty;
        Email = string.Empty;
        PhoneNumber = string.Empty;
    }

    private async Task ShowTemporaryMessageAsync(string message, Color color)
    {
        Message = message;
        MessageColor = color;
        IsMessageVisible = true;
        await Task.Delay(3000);
        IsMessageVisible = false;
    }
}
