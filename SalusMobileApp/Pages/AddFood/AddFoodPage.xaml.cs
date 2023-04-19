using SalusMobileApp.Data;
using SalusMobileApp.Models;

namespace SalusMobileApp.Pages.AddFood;

public partial class AddFoodPage : ContentPage
{
    private RecipeModel _recipe;
	public AddFoodPage(RecipeModel recipe)
	{
		InitializeComponent();
        _recipe = recipe;
        kcalEntry.Text = _recipe.kcal.ToString();
        proteinEntry.Text = _recipe.protein.ToString();
        carbohydrateEntry.Text = _recipe.carbohydrate.ToString();
    }

    private async void enableScannerButton_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new FoodScannerPage());
    }

    private void addFoodButton_Clicked(object sender, EventArgs e)
    {
        App.mostRecentRecipe = new RecipeModel(0, 0, 0);
    }
}