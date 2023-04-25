using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.AddFood;

public partial class AddIngredientPage : ContentPage
{
	GetRecipeByNameViewModel viewModel;
    List<int> addedIngredientIds = new List<int>();
    List<int> addedIngredientGrams = new List<int>();
    public AddIngredientPage()
	{
		InitializeComponent();
		viewModel = new GetRecipeByNameViewModel();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = viewModel;
        ingredientList.ItemsSource = viewModel.Recipes;
    }

    private void searchIngredientsButton_Clicked(object sender, EventArgs e)
    {
		if(ServiceValidation.InternetConnectionValidator())
		{
            viewModel.GetRecipeByName(searchIngredientsEntry.Text);
            viewModel.RecipeLoaded += (sender, e) =>
            ingredientList.ItemsSource = viewModel.Recipes;
            //
        }
    }

    private async void ingredientList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if(ingredientList.SelectedItem != null) 
        {
            var gramPerPortion = await DisplayPromptAsync("Add ingredient", "How much of this ingredient is in 1 portion?", "Add", "Cancel", "Answer in grams", keyboard: Keyboard.Numeric);
            if (gramPerPortion != null)
            {
                ComplexRecipeModel selected = (ComplexRecipeModel)ingredientList.SelectedItem;
                addedIngredientsList.Text += selected.name + " " + gramPerPortion + "g, ";
                addedIngredientIds.Add(selected.id);
                addedIngredientGrams.Add(Convert.ToInt32(gramPerPortion));
            }
        }
    }

    private async void completeAddingIngredients_Clicked(object sender, EventArgs e)
    {
        if(await DisplayAlert("Complete", "Are you sure you want to go back to the previous page", "Yes", "No"))
        {
            App.currentAddedIngredientIds = addedIngredientIds;
            App.currentAddedIngredientGrams = addedIngredientGrams;
            await Navigation.PopAsync();
        }
    }

    private async void deleteIngredientsButton_Clicked(object sender, EventArgs e)
    {
        if(await DisplayAlert("Delete all", "This action will delete all of your added ingredients. Are you sure?", "Yes", "No"))
        {
            App.currentAddedIngredientIds = null;
            App.currentAddedIngredientGrams = null;
            ingredientList.SelectedItem = null;
        }
    }
}