using CommunityToolkit.Mvvm.ComponentModel;

namespace ContactMauiApplication.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? message;

        [ObservableProperty]
        private Color messageColor = Colors.Transparent;

        [ObservableProperty]
        private bool isMessageVisible;

        protected async Task ShowTemporaryMessageAsync(string newMessage, Color newColor)
        {
            Message = newMessage;
            MessageColor = newColor;
            IsMessageVisible = true;
            await Task.Delay(3000);
            IsMessageVisible = false;
        }
    }
}
