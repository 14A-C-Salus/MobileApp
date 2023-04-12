namespace SalusMobileApp;
using SalusMobileApp.Data;
using SalusMobileApp.Pages.Login_Signup;
using SalusMobileApp.Pages;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

    private async void loginSignupNavigationButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginSignupPage());
        //await Shell.Current.GoToAsync(nameof(LoginSignupPage));
    }
}

