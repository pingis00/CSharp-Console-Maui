using ContactMauiApplication.ViewModels;

namespace ContactMauiApplication.Pages;

public partial class UpdateContactPage : ContentPage
{
	public UpdateContactPage(UpdateContactViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        viewModel.PropertyChanged += async (s, e) =>
        {
            if (e.PropertyName == nameof(viewModel.IsMessageVisible))
            {
                if (viewModel.IsMessageVisible)
                {
                    await MessageLabel.FadeTo(1, 500);
                }
                else
                {
                    await MessageLabel.FadeTo(0, 500);
                }
            }
        };
    }
}