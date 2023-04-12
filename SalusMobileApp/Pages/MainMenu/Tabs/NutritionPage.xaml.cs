using SalusMobileApp.Pages.AddFood;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.Tabs;

public partial class NutritionPage : ContentPage
{
	public NutritionPage()
	{
		InitializeComponent();
	}

    private async void addNewButton_Clicked(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync(nameof(AddFoodPage));
        await Navigation.PushAsync(new AddFoodPage());
    }
}