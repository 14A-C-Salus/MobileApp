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
		id = userId;
	}
}