using SalusMobileApp.Pages.Login_Signup;
using System.Formats.Asn1;

namespace SalusMobileApp.Pages.Error;

public partial class ErrorPage : ContentPage
{
	public ErrorPage()
	{
		InitializeComponent();
		errorMessageLabel.Text = "We have encountered an error while processing your request. You will be redirected to the login page.";
	}

    private async void backToLoginButton_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync(nameof(LoginSignupPage));
    }
}