using CommunityToolkit.Mvvm.ComponentModel;

namespace ContactMauiApplication.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        public string? message;
    }
}
