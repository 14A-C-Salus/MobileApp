using SalusMobileApp.Pages.UserProfile;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.Tabs;

public partial class ProfilePage : ContentPage
{
    ProfilePageViewModel viewModel;
	public ProfilePage()
	{
		InitializeComponent();
        viewModel = new ProfilePageViewModel();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = viewModel;
        viewModel.GetUserProfileFromViewModel();
        BindableLayout.SetItemsSource(userProfileBindableLayout, viewModel.UserProfile);
    }

    private async void editButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(EditProfilePage));
    }
}