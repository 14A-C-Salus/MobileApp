using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.SocialPages;

public partial class SelectedUserRecipesPage : ContentPage
{
	GetRecipeByAuthIdViewModel viewModel;
	public SelectedUserRecipesPage(int userId)
	{
		InitializeComponent();
		viewModel = new GetRecipeByAuthIdViewModel();
		BindingContext = viewModel;
        viewModel.GetRecipeById(userId);
        viewModel.RecipeLoaded += (sender, e) =>
        recipesList.ItemsSource = viewModel.Recipes;
    }
}