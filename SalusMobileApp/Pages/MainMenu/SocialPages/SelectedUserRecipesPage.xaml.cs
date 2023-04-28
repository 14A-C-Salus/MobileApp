using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.SocialPages;

public partial class SelectedUserRecipesPage : ContentPage
{
	GetRecipeByAuthIdViewModel viewModel;
    private int id;
	public SelectedUserRecipesPage(int userId)
	{
		InitializeComponent();
		viewModel = new GetRecipeByAuthIdViewModel();
		BindingContext = viewModel;
        viewModel.GetRecipeById(userId);
        id = userId;
        viewModel.RecipeLoaded += (sender, e) =>
        recipesList.ItemsSource = viewModel.Recipes;
    }

    private async void likeRecipeButton_Clicked(object sender, EventArgs e)
    {
        if(ServiceValidation.InternetConnectionValidator())
        {
            if(recipesList.SelectedItem != null)
            {
                var selected = (ComplexRecipeModel)recipesList.SelectedItem;
                var likeUnlike = await RestServices.RecipeLikeUnlike(selected.id);
                if(likeUnlike)
                {
                    viewModel.GetRecipeById(id);
                    recipesList.ItemsSource = viewModel.Recipes;
                }
            }
            else
            {
                await DisplayAlert("Error", "Please select a recipe to like", "Ok");
            }
        }
    }
}