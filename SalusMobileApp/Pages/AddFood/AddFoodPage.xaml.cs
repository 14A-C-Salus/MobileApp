using SalusMobileApp.Data;

namespace SalusMobileApp.Pages.AddFood;

public partial class AddFoodPage : ContentPage
{
	public AddFoodPage()
	{
		InitializeComponent();
	}

    private async void enableScannerButton_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new FoodScannerPage());
    }
}