using SalusMobileApp.Data;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.SocialPages;

public partial class SelectedUserProfilePage : ContentPage
{
	ProfilePageViewModel viewModel;
    private int id;
	public SelectedUserProfilePage(int userId)
	{
		InitializeComponent();
        viewModel = new ProfilePageViewModel();
        BindingContext = viewModel;
        viewModel.GetUserProfileFromViewModel(userId);
        viewModel.UserProfileLoaded += (sender, e) =>
        BindableLayout.SetItemsSource(userProfileBindableLayout, viewModel.UserProfile);
        id = userId;
    }

    private async void userRecipesButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SelectedUserRecipesPage(id));
    }

    private void userComments_Clicked(object sender, EventArgs e)
    {

    }
}