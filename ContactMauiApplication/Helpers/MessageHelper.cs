using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace ContactMauiApplication.Helpers;

public static class MessageHelper
{
    [ObservableProperty]
    private string? message;

    [ObservableProperty]
    private Color messageColor = Colors.Transparent;

    [ObservableProperty]
    private bool isMessageVisible;
    public static async Task ShowTemporaryMessage(string message, Color color)
    {
        Message = message;
        MessageColor = color;
        IsMessageVisible = true;
        await Task.Delay(3000);
        IsMessageVisible = false;
    }
}
