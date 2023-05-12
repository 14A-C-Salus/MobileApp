using SalusMobileApp.Data;

namespace SalusMobileApp.Pages.AddFood;

public partial class FoodScannerPage : ContentPage
{
	public FoodScannerPage()
	{
		InitializeComponent();
        bool fgh = MainThread.IsMainThread;
    }

    private async void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        bool fgh = MainThread.IsMainThread;
        var barcode = e.Results[0].Value;
        barcodeReader.IsDetecting = false;
        var doesBarcodeExist = await RestServices.GetFoodInformationByBarcode(barcode);
        if(doesBarcodeExist)
        {
            if(!MainThread.IsMainThread)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new AddFoodPage());
                });
            }
        }
        else
        {
        }
        await Navigation.PopAsync();
    }
}