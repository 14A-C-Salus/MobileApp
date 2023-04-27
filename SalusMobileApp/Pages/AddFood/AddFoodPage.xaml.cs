using SalusMobileApp.Data;
using SalusMobileApp.Models;

namespace SalusMobileApp.Pages.AddFood;

public partial class AddFoodPage : ContentPage
{
    private bool AdvancedCreation = false;
    private string CreateErrorMessage = "Something went wrong, please make sure that you filled out everything correctly!";
    private string CreateSuccessMessage = "Your new recipe has been successfully created! Do you wish to add it to your favourites?";

    public AddFoodPage(string name = "", string kcal = "", string protein = "", string fat = "", string carbohydrate = "")
	{
		InitializeComponent();
        nameEntry.Text = name;
        kcalEntry.Text = kcal.ToString();
        proteinEntry.Text = protein.ToString();
        fatEntry.Text = fat.ToString();
        showAdvancedButton.IsEnabled = true;
        showAdvancedButton.IsVisible = true;
    }

    private async void enableScannerButton_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new FoodScannerPage());
    }

    private async void addFoodButton_Clicked(object sender, EventArgs e)
    {
        if(ServiceValidation.InternetConnectionValidator())
        {
            if(AdvancedCreation && !foodIsScanned.IsVisible)
            {
                var createRecipe = await RestServices.CreateRecipe(App.currentAddedIngredientIds.ToArray(), App.currentAddedIngredientGrams.ToArray(), methodPicker.SelectedIndex, oilPicker.SelectedIndex, Convert.ToInt32(oilMlEntry.Text), Convert.ToInt32(cookingTimeEntry.Text), nameEntry.Text, generateDescriptionIsChecked.IsChecked, descriptionEntry.Text);
                if(!createRecipe)
                {
                    await DisplayAlert("Error", CreateErrorMessage, "Ok");
                    if (oilPicker.SelectedItem.ToString() == "Frying")
                    {
                        oilErrorMessage.IsVisible = true;
                    }
                }
                else
                {
                    if(await DisplayAlert("Success", CreateSuccessMessage, "Yes", "No"))
                    {
                        App.database.SaveRecipeToLocalDatabase(App.saveLocalRecipe);
                    }
                    await Navigation.PopAsync();
                }
            }
            else
            {
                var createRecipe = await RestServices.CreateRecipeSimple(nameEntry.Text, Convert.ToInt32(kcalEntry.Text), Convert.ToInt32(proteinEntry.Text), Convert.ToInt32(fatEntry.Text), Convert.ToInt32(carbohydrateEntry.Text));
                if(!createRecipe)
                {
                    await DisplayAlert("Error", CreateErrorMessage, "Ok");
                }
                else
                {
                    if(await DisplayAlert("Success", CreateSuccessMessage, "Yes", "No"))
                    {
                        App.database.SaveRecipeToLocalDatabase(App.saveLocalRecipe);
                    }
                    await Navigation.PopAsync();
                }
            }
        }
        else
        {
            await DisplayAlert("Error", "This feature isn't available while you are offline. Please try again later!", "Ok");
            await Navigation.PopAsync();
        }
        App.mostRecentRecipe = new RecipeModel("", 0, 0, 0, 0);
    }

    private void showAdvancedButton_Clicked(object sender, EventArgs e)
    {
        if (cookingMethodLayout.IsVisible && cookingTimeLayout.IsVisible && descriptionLayout.IsVisible && oilLayout.IsVisible && ingredientList.IsVisible && AdvancedCreation)
        {
            cookingMethodLayout.IsVisible = false;
            cookingTimeLayout.IsVisible = false;
            descriptionLayout.IsVisible = false;
            oilLayout.IsVisible = false;
            ingredientList.IsVisible = false;
            AdvancedCreation = false;
            showAdvancedButton.BackgroundColor = Colors.Gray;
        }
        else
        {
            cookingMethodLayout.IsVisible = true;
            cookingTimeLayout.IsVisible = true;
            descriptionLayout.IsVisible = true;
            oilLayout.IsVisible = true;
            ingredientList.IsVisible = true;
            AdvancedCreation = true;
            showAdvancedButton.BackgroundColor = Colors.Green;
        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(generateDescriptionIsChecked.IsChecked)
        {
            descriptionEntry.IsEnabled = false;
        }
        else
        {
            descriptionEntry.IsEnabled = true;
        }
    }

    private async void addIngredient_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddIngredientPage());
    }

    private void oilPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        oilErrorMessage.IsVisible = false;
    }

    private void viewIngredients_Clicked(object sender, EventArgs e)
    {
        ingredientListLabel.Text = string.Empty;
        if (App.currentAddedIngredientIds.Count > 0)
        {
            for (int i = 0; i < App.currentAddedIngredientIds.Count; i++)
            {
                ingredientListLabel.Text += App.currentAddedIngredientNames[i] + " " + App.currentAddedIngredientGrams[i] + "g\n";

            }
        }
    }

    private async void cancelButton_Clicked(object sender, EventArgs e)
    {
        App.mostRecentRecipe = new RecipeModel("", 0, 0, 0, 0);
        App.currentAddedIngredientIds.Clear();
        App.currentAddedIngredientGrams.Clear();
        App.currentAddedIngredientNames.Clear();
        await Navigation.PopAsync();
    }
}