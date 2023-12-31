using ContactMauiApplication.Pages;

namespace ContactMauiApplication;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("deletecontactpage", typeof(DeleteContactPage));
        Routing.RegisterRoute("updatecontactpage", typeof(UpdateContactPage));

    }
}
