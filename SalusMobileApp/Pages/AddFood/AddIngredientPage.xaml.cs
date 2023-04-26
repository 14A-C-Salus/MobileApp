using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.AddFood;

public partial class AddIngredientPage : ContentPage
{
	GetRecipeByNameViewModel viewModel;
    List<int> addedIngredientIds = new List<int>();
    List<int> addedIngredientGrams = new List<int>();
    List<string> addedIngredientNames = new List<string>();
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
                addedIngredientsList.Text += selected.name + " " + gramPerPortion + "g,\n";
                int rowHeight = Convert.ToInt32(addedIngredientListRow.Height.Value);
                addedIngredientListRow.Height = rowHeight + 15;
                addedIngredientIds.Add(selected.id);
                addedIngredientGrams.Add(Convert.ToInt32(gramPerPortion));
                addedIngredientNames.Add(selected.name);
            }
        }
    }

    private async void completeAddingIngredients_Clicked(object sender, EventArgs e)
    {
        if(await DisplayAlert("Complete", "Did you add every ingredient you wanted?", "Yes", "No"))
        {
            for (int i = 0; i < addedIngredientIds.Count; i++)
            {
                App.currentAddedIngredientIds.Add(addedIngredientIds[i]);
            }
            for (int i = 0; i < addedIngredientGrams.Count; i++)
            {
                App.currentAddedIngredientGrams.Add(addedIngredientGrams[i]);
            }
            for (int i = 0; i < addedIngredientNames.Count; i++)
            {
                App.currentAddedIngredientNames.Add(addedIngredientNames[i]);
            }
            await Navigation.PopAsync();
        }
    }

    private async void deleteIngredientsButton_Clicked(object sender, EventArgs e)
    {
        if(await DisplayAlert("Delete all", "This action will delete all of your added ingredients. Are you sure?", "Yes", "No"))
        {
            App.currentAddedIngredientIds.Clear();
            App.currentAddedIngredientGrams.Clear();
            App.currentAddedIngredientNames.Clear();
            addedIngredientIds.Clear();
            addedIngredientGrams.Clear();
            addedIngredientNames.Clear();
            ingredientList.SelectedItem = null;
            addedIngredientsList.Text = string.Empty;
            addedIngredientListRow.Height = 25;
        }
    }

    private async void exitButton_Clicked(object sender, EventArgs e)
    {
        if(await DisplayAlert("Quit", "Are you sure? Your changes won't be saved.", "Yes", "No"))
        {
            addedIngredientIds.Clear();
            addedIngredientGrams.Clear();
            addedIngredientNames.Clear();
            await Navigation.PopAsync();
        }
    }
}