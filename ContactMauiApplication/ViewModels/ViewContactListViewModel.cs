using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ContactMauiApplication.ViewModels;

public partial class ViewContactListViewModel : ObservableObject
{
    [ObservableProperty]
    private string message;

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
        Task task = LoadContacts();
    }

    public async Task LoadContacts()
    {
        var result = await _contactService.GetContactsFromListAsync();
        if (result.Status == ServiceStatus.SUCCESS && result.Result is List<IContact> contactList)
        {
            Contacts = new ObservableCollection<IContact>(contactList);
            IsMessageVisible = false;
        }
        else
        {
            Contacts = new ObservableCollection<IContact>();
            Message = "Listan är tom eller kunde inte laddas.";
            MessageColor = Colors.Red;
            IsMessageVisible = true;
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

    private async void OnContactsUpdated(object sender, EventArgs e)
    {
        await LoadContacts();
    }

    [RelayCommand]
    public async Task NavigateToDeleteContactPage()
    {
        await Shell.Current.GoToAsync("deletecontactpage");
    }
}
