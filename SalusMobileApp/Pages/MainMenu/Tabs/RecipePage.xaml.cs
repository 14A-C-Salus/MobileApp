using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.Pages.AddFood;
using SalusMobileApp.ViewModels;
using System.Security.Cryptography;
using System.Windows.Input;

namespace SalusMobileApp.Pages.MainMenu.Tabs;

public partial class RecipePage : ContentPage
{
    GetRecipeByAuthIdViewModel viewModel;
    public RecipePage()
	{
		InitializeComponent();
        viewModel = new GetRecipeByAuthIdViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = viewModel;
        viewModel.GetRecipeById(Convert.ToInt32(App._userProfile.id));
        viewModel.RecipeLoaded += (sender, e) =>
        recipeList.ItemsSource = viewModel.Recipes;
    }

    private void searchButton_Clicked(object sender, EventArgs e)
    {
        viewModel.GetRecipeById(Convert.ToInt32(App._userProfile.id));
        viewModel.RecipeLoaded += (sender, e) =>
        recipeList.ItemsSource = viewModel.Recipes.Where(s => s.name.Contains(searchEntry.Text));
    }

    private async void addNewButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddFoodPage());
    }

    private void filterByFavourites_Clicked(object sender, EventArgs e)
    {
        viewModel.GetRecipeFromLocalDatabase();
        viewModel.RecipeLoaded += (sender, e) =>
        recipeList.ItemsSource = viewModel.FavouriteRecipes;
    }

    private void reloadButton_Clicked(object sender, EventArgs e)
    {
        viewModel.GetRecipeById(Convert.ToInt32(App._userProfile.id));
        viewModel.RecipeLoaded += (sender, e) =>
        recipeList.ItemsSource = viewModel.Recipes;
    }

    private async void favouriteRecipeButton_Clicked(object sender, EventArgs e)
    {
        if (recipeList.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select a recipe by tapping it first!", "Ok");
        }
        else
        {
            var selectedRecipe = (ComplexRecipeModel)recipeList.SelectedItem;
            int selectedMethod = 0;
            if (selectedRecipe.method != null)
            {
                selectedMethod = (int)selectedRecipe.method;
            }
            int selectedOilMl = 0;
            if (selectedRecipe.oilPortionMl != null)
            {
                selectedOilMl = (int)selectedRecipe.method;
            }
            //var ingredientsString = "";
            //if (selectedRecipe.ingredients.Count() > 0)
            //{
            //    for (int i = 0; i < selectedRecipe.ingredients.Count(); i++)
            //    {
            //        ingredientsString = selectedRecipe.ingredients[i] + " " + selectedRecipe.ingredients[i] + "g\n";
            //    }
            //}
            //RecipeModel recipeToLocal = new RecipeModel
            //{
            //    method = selectedMethod,
            //    oilId = selectedRecipe.oilId,
            //    oilPortionMl = selectedRecipe.oilPortionMl,
            //    timeInMinutes = selectedRecipe.timeInMinutes,
            //    name = selectedRecipe.name,
            //    description = selectedRecipe.description,
            //    fat = selectedRecipe.fat,
            //    protein = selectedRecipe.protein,
            //    kcal = selectedRecipe.kcal,
            //    carbohydrate = selectedRecipe.carbohydrate,
            //    ingredientsString = ingredientsString
            //};
        }
    }

    private async void editRecipe_Clicked(object sender, EventArgs e)
    {
        if (recipeList.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select a recipe by tapping it first!", "Ok");
        }
    }

    private async void deleteRecipe_Clicked(object sender, EventArgs e)
    {
        if (recipeList.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select a recipe by tapping it first!", "Ok");
        }
        else
        {
            var selected = (ComplexRecipeModel)recipeList.SelectedItem;
            var delete = await RestServices.DeleteRecipe(selected.id);
            if(delete)
            {
                await DisplayAlert("Success", "Recipe successfully deleted.", "Ok");
            }
            else
            {
                await DisplayAlert("Error", "Something went wrong.", "Ok");
            }
        }
    }
}