using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.MealPages;

public partial class AddMealsPages : ContentPage
{
    GetRecipeByNameViewModel viewModel;
    private int selectedRecipeId =  0;
    private int selectedRecipePortion = 0;
    public AddMealsPages()
	{
		InitializeComponent();
        viewModel = new GetRecipeByNameViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = viewModel;
        recipeListView.ItemsSource = viewModel.Recipes;
    }

    private void searchRecipesButton_Clicked(object sender, EventArgs e)
    {
        if (ServiceValidation.InternetConnectionValidator())
        {
            viewModel.GetRecipeByName(searchRecipesEntry.Text);
            recipeListView.ItemsSource = viewModel.Recipes;
        }
    }

    private async void addMealButton_Clicked(object sender, EventArgs e)
    {
        if (ServiceValidation.InternetConnectionValidator())
        {
            if(selectedRecipeId != 0 && selectedRecipePortion != 0)
            {
                if(await DisplayAlert("Add", $"Do you want to add {selectedRecipePortion}g of {selectedRecipeLabel.Text} to your meals?", "Yes", "No"))
                {
                    NewMeal meal = new NewMeal();
                    if (int.TryParse(dlEntry.Text, out int dlValue)) 
                    {
                        meal = new NewMeal
                        {
                            isLiquid = isLiquidCheckbox.IsChecked,
                            recipeId = selectedRecipeId,
                            portion = selectedRecipePortion,
                            dl = dlValue
                        };
                    }
                    else
                    {
                        meal = new NewMeal
                        {
                            isLiquid = isLiquidCheckbox.IsChecked,
                            recipeId = selectedRecipeId,
                            portion = selectedRecipePortion
                        };
                    }
                    var postMeal = await RestServices.PostLast24h(meal);
                    if(postMeal)
                    {
                        await DisplayAlert("Success", "You have successfully added a new meal!", "Ok");
                        await Navigation.PushAsync(new MainMenuPage());
                    }
                    else
                    {
                        await DisplayAlert("Error", "Something went wrong while we processed your request.", "Ok");
                    }
                }
            }
        }
        recipeListView.SelectedItem = null;
    }

    private async void recipeListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var portion = await DisplayPromptAsync("Select", "How much did you eat?", "Add", "Cancel", "Answer in grams", keyboard: Keyboard.Numeric);
        if (portion != null)
        {
            if(recipeListView.SelectedItem != null)
            {
                var selected = (ComplexRecipeModel)recipeListView.SelectedItem;
                selectedRecipeId = selected.id;
                selectedRecipeLabel.Text = selected.name;
                selectedRecipePortion = Convert.ToInt32(portion);
            }
        }
    }

    private void isLiquidCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(isLiquidCheckbox.IsChecked)
        {
            dlEntry.IsEnabled = true;
        }
        else
        {
            dlEntry.IsEnabled = false;
        }
    }

    private async void exitButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}