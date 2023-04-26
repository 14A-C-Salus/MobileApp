using SalusMobileApp.Data;
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
        viewModel.GetUserProfileFromViewModel(int.Parse(App.userId));
        viewModel.UserProfileLoaded += (sender, e) =>
        BindableLayout.SetItemsSource(userProfileBindableLayout, viewModel.UserProfile);
    }
    
    private async void editButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditProfilePage());
    }

    private async void logoutButton_Clicked(object sender, EventArgs e)
    {
        var logoutAlert = await DisplayAlert("Logout", "Are you sure?", "Yes", "No");
        if(logoutAlert)
        {
            App.database.DeleteAll();
            if(ServiceValidation.IsEverythingDeleted())
            {
                Application.Current.Quit();
            }
        }
    }
}