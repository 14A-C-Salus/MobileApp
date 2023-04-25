using SalusMobileApp.Data;

namespace SalusMobileApp.Pages.AddFood;

public partial class FoodScannerPage : ContentPage
{
	public FoodScannerPage()
	{
		InitializeComponent();
	}

    private async void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var barcode = e.Results[0].Value;
        barcodeReader.IsDetecting = false;
        var doesBarcodeExist = await RestServices.GetFoodInformationByBarcode(barcode);
        if(doesBarcodeExist)
        {
            await Navigation.PushAsync(new AddFoodPage(App.mostRecentRecipe.name, App.mostRecentRecipe.kcal.ToString(), App.mostRecentRecipe.protein.ToString(), App.mostRecentRecipe.fat.ToString(), App.mostRecentRecipe.carbohydrate.ToString()));
        }
        else
        {
            await Navigation.PopAsync();
        }
    }
}