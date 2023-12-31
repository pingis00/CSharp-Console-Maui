using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using System.Collections.ObjectModel;

namespace ContactMauiApplication.ViewModels;

public partial class ViewContactListViewModel : ObservableObject
{
    private readonly IContactService _contactService;

    [ObservableProperty]
    public ObservableCollection<IContact> _contacts;

    public ViewContactListViewModel(IContactService contactService)
    {
        _contactService = contactService;
        _contactService.ContactsUpdated += OnContactsUpdated;
        LoadContacts();
    }

    public async Task LoadContacts()
    {
        var result = await _contactService.GetContactsFromListAsync();
        if (result.Status == ServiceStatus.SUCCESS && result.Result is List<IContact> contactList)
        {
            Contacts = new ObservableCollection<IContact>(contactList);
        }
    }

    private async void OnContactsUpdated(object sender, EventArgs e)
    {
        await LoadContacts();
    }

    [RelayCommand]
    private async Task NavigateToDeleteContact()
    {
        await Shell.Current.GoToAsync("deletecontactpage");
    }
}
