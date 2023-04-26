using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.SocialPages;

public partial class SelectedUserProfilePage : ContentPage
{
	ProfilePageViewModel viewModel;
	public SelectedUserProfilePage(int userId)
	{
		InitializeComponent();
        viewModel = new ProfilePageViewModel();
        BindingContext = viewModel;
        viewModel.GetUserProfileFromViewModel(userId);
        viewModel.UserProfileLoaded += (sender, e) =>
        BindableLayout.SetItemsSource(userProfileBindableLayout, viewModel.UserProfile);
    }
}