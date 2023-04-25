using SalusMobileApp.Data;
using SalusMobileApp.Models;

namespace SalusMobileApp.Pages.AddFood;

public partial class AddFoodPage : ContentPage
{
    private bool AdvancedCreation = false;
    private string CreateErrorMessage = "Something went wrong, please make sure that you filled out everything correctly!";
    private string CreateSuccessMessage = "Your new recipe has been successfully created! Do you wish to save it to your favourites?";

    public AddFoodPage(string name = "", string kcal = "", string protein = "", string fat = "", string carbohydrate = "")
	{
		InitializeComponent();
        nameEntry.Text = name;
        kcalEntry.Text = kcal.ToString();
        proteinEntry.Text = protein.ToString();
        fatEntry.Text = fat.ToString();
        carbohydrateEntry.Text = carbohydrate.ToString();
        if(name != "" && kcal != "" && protein != "" && fat != "" &&  carbohydrate != "")
        {
            showAdvancedButton.IsVisible = false;
            showAdvancedButton.IsEnabled = false;
            foodIsScanned.IsVisible = true;
        }
        if(App.currentAddedIngredientIds.Count > 0)
        {
            foreach (var ingredient in App.currentAddedIngredientIds)
            {
                ingredientListLabel.Text += App.currentAddedIngredientIds[ingredient] + App.currentAddedIngredientGrams[ingredient] + "\n";
            }
        }
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
                var createRecipe = await RestServices.CreateRecipe(App.currentAddedIngredientIds.ToArray(), App.currentAddedIngredientGrams.ToArray(), methodPicker.SelectedIndex, oilPicker.SelectedIndex,Convert.ToInt32(oilMlEntry.Text), Convert.ToInt32(cookingTimeEntry.Text), nameEntry.Text, generateDescriptionIsChecked.IsChecked, descriptionEntry.Text, Convert.ToInt32(fatEntry.Text), Convert.ToInt32(proteinEntry.Text), Convert.ToInt32(kcalEntry.Text), Convert.ToInt32(carbohydrateEntry.Text));
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
        cookingMethodLayout.IsVisible = true;
        cookingTimeLayout.IsVisible = true;
        descriptionLayout.IsVisible = true;
        oilLayout.IsVisible = true;
        ingredientList.IsVisible = true;
        AdvancedCreation = true;
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
}