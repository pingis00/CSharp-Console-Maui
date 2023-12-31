using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ContactMauiApplication.ViewModels;

public partial class HomePageViewModel : ObservableObject
{
    [RelayCommand]
    private async Task NavigateToAddContact()
    {
        await Shell.Current.GoToAsync("AddContactPage");
    }

    [RelayCommand]
    private async Task NavigateToViewContactListPage()
    {
        await Shell.Current.GoToAsync("ViewContactListPage");
    }
}
