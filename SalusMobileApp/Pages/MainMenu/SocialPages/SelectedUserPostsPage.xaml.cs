using Newtonsoft.Json.Linq;
using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.SocialPages;

public partial class SelectedUserPostsPage : ContentPage
{
	CommentsViewModel viewModel;
	private int id;
	public SelectedUserPostsPage(int userId)
	{
		InitializeComponent();
		viewModel = new CommentsViewModel();
		BindingContext = viewModel;
		viewModel.GetCommentsById(userId);
        viewModel.CommentsLoaded += (sender, e) =>
        postsListView.ItemsSource = viewModel.Comments;
        id = userId;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		if (ServiceValidation.InternetConnectionValidator())
		{
			var comment = await DisplayPromptAsync("Write", "Do you want to send this comment?", "Yes", "No", "Type here");
			if(comment != null)
			{
				if(id == App._userProfile.id)
				{
					var userEmail = await RestServices.GetProfileData(int.Parse(App.userId));
					if(userEmail != null)
					{
						var sendComment = await RestServices.SaveComment(userEmail["email"].ToString(), comment);
						if (sendComment)
						{
							viewModel.GetCommentsById(id);
							postsListView.ItemsSource = viewModel.Comments;
						}
					}
                }
				else
				{
					var sendComment = await RestServices.SaveComment(App.selectedPersonEmail, comment);
					if(sendComment)
					{
						viewModel.GetCommentsById(id);
						postsListView.ItemsSource = viewModel.Comments;
					}
				}
			}
		}
		else
		{
			await DisplayAlert("Error", "No internet connection!", "Ok");
		}
    }

    private async void postsListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
		var selected = (CommentModel)postsListView.SelectedItem;
		if(selected != null)
		{
			if(selected.fromId == selected.toId)
			{
                await DisplayAlert("Post", $"{selected.body}", "Close");
            }
			else
			{
                await DisplayAlert($"From {selected.email}", $"{selected.body}", "Close");
				postsListView.SelectedItem = null;
            }
        }
        
    }
}