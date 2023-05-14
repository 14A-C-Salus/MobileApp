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
            if(!MainThread.IsMainThread)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if(App.mostRecentRecipe != null)
                    {
                        var rec = App.mostRecentRecipe;
                        await Navigation.PushAsync(new AddFoodPage(rec.name, rec.kcal.ToString(), rec.protein.ToString(), rec.fat.ToString(), rec.carbohydrate.ToString() ));
                    }
                });
            }
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopAsync();
            });
        }
        
    }
}