using ContactMauiApplication.ViewModels;

namespace ContactMauiApplication.Pages;

public partial class ViewContactListPage : ContentPage
{
	public ViewContactListPage(ViewContactListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}