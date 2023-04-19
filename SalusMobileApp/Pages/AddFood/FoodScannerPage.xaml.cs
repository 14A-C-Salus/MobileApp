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
        await RestServices.GetFoodInformationByBarcode(barcode);
        await Navigation.PushAsync(new AddFoodPage(App.mostRecentRecipe));
    }
}