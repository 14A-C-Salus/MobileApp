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

    private async void ownProfile_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SelectedUserPostsPage(App._userProfile.id));
    }

    private async void commentListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if(await DisplayAlert(" ", "Do you want to go to this person's profile?", "Yes", "No"))
        {
            if(profilesListView.SelectedItem != null)
            {
                var selected = (UserDataModel)profilesListView.SelectedItem;
                var selectedId = selected.userProfile.id;
                App.selectedPersonEmail = selected.email;
                try
                {
                    await Navigation.PushAsync(new SelectedUserProfilePage(selectedId));
                }
                catch(Exception ex)
                {
                    await DisplayAlert("Error", "This person doesn't have a confirmed profile yet, you can't view it", "Cancel");
                }
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
            profilesListView.ItemsSource = viewModel.UserProfiles.Where(u => u.isAdmin == true);
        }
        else
        {
            profilesListView.ItemsSource = viewModel.UserProfiles;
        }
    }
}