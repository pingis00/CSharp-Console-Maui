using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using System.Collections.ObjectModel;
using Contact = ContactServiceLibrary.Models.Contact;

namespace ContactMauiApplication.ViewModels;

public partial class ViewContactListViewModel : ObservableObject
{
    [ObservableProperty]
    private string? message;

    [ObservableProperty]
    private Color messageColor = Colors.Transparent;

    [ObservableProperty]
    private bool isMessageVisible;

    private readonly IContactService _contactService;

    [ObservableProperty]
    public ObservableCollection<IContact>? _contacts;

    public ViewContactListViewModel(IContactService contactService)
    {
        _contactService = contactService;
        _contactService.ContactsUpdated += OnContactsUpdated;
        _ = LoadContacts();
    }

    public async Task LoadContacts()
    {
        var result = await _contactService.GetContactsFromListAsync();
        if (result.Status == ServiceStatus.SUCCESS && result.Result is List<IContact> contactList)
        {
            Contacts = new ObservableCollection<IContact>(contactList);
            if (contactList.Count == 0)
            {
                Message = "Listan är tom.";
                MessageColor = Colors.Red;
                IsMessageVisible = true;
            }
            else
            {
                IsMessageVisible = false;
            }
        }
        else
        {
            Message = "Kunde inte ladda listan.";
            MessageColor = Colors.Red;
            IsMessageVisible = true;
        }
    }

    private async void OnContactsUpdated(object? sender, EventArgs e)
    {
        await LoadContacts();
    }

    protected async Task ShowTemporaryMessageAsync(string message, Color color)
    {
        Message = message;
        MessageColor = color;
        IsMessageVisible = true;
        await Task.Delay(3000);
        IsMessageVisible = false;
    }

    [RelayCommand]
    public async Task NavigateToDeleteContactPage(Contact contact)
    {
        var parameters = new ShellNavigationQueryParameters
        {
            { "Contact", contact }
        };

        await Shell.Current.GoToAsync("deletecontactpage", parameters);
    }

    [RelayCommand]
    public async Task NavigateToUpdateContactPage(Contact contact)
    {
        var parameters = new ShellNavigationQueryParameters
        {
            { "Contact", contact }
        };

        await Shell.Current.GoToAsync("updatecontactpage", parameters);
    }
}
