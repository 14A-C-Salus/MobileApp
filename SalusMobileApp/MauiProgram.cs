using SalusMobileApp.Data;
using SalusMobileApp.Pages.Login_Signup;
using ZXing.Net.Maui.Controls;

namespace SalusMobileApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>().UseBarcodeReader()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<RestServices>();
		builder.Services.AddSingleton<LoginSignupPage>();

		return builder.Build();
	}
}
