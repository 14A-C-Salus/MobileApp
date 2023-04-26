using SalusMobileApp.Models;
using SalusMobileApp.Pages.MainMenu.SocialPages;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.Tabs;

public partial class SocialPage : ContentPage
{
	GetUserProfilesByNameViewModel viewModel;
	public SocialPage()
	{
		InitializeComponent();
		viewModel = new GetUserProfilesByNameViewModel();
		BindingContext = viewModel;
	}

    private async void searchButton_Clicked(object sender, EventArgs e)
    {
        var getUsers = await viewModel.GetUserProfilesByName(searchEntry.Text);
        if(getUsers)
        {
            LoadData(adminsOnly.IsChecked);
        }
    }

    private void ownProfile_Clicked(object sender, EventArgs e)
    {

    }

    private async void commentListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if(await DisplayAlert(" ", "Do you want to go to this person's profile?", "Yes", "No"))
        {
            if(profilesListView.SelectedItem != null)
            {
                var selected = (UserDataModel)profilesListView.SelectedItem;
                var selectedId = selected.id;
                await Navigation.PushAsync(new SelectedUserProfilePage(selectedId));
            }
            else
            {
                await DisplayAlert("Error", "Something went wrong while loading the selected user's profile.", "Ok");
            }
        }
        else
        {
            profilesListView.SelectedItem = null;
        }
    }

    private void LoadData(bool isAdminChecked)
    {
        if (isAdminChecked)
        {
            viewModel.UserProfilesLoaded += (sender, e) =>
            profilesListView.ItemsSource = viewModel.UserProfiles.Where(u => u.isAdmin == true);
        }
        else
        {
            viewModel.UserProfilesLoaded += (sender, e) =>
            profilesListView.ItemsSource = viewModel.UserProfiles;
        }
    }
}