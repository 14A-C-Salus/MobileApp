using SalusMobileApp.Data;
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
				var sendComment = await RestServices.SaveComment(App.selectedPersonEmail, comment);
				if(sendComment)
				{
                    viewModel.GetCommentsById(id);
                    postsListView.ItemsSource = viewModel.Comments;
                }
			}
		}
		else
		{
			await DisplayAlert("Error", "No internet connection!", "Ok");
		}
    }
}