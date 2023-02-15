using Microsoft.Maui.Networking;
using SalusMobileApp.Data;
using SalusMobileApp.Pages.Login_Signup;

namespace SalusMobileApp.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if(accessType == NetworkAccess.Internet) 
        {
            if (ServiceValidation.validateLoginData(emailEntry.Text, passwordEntry.Text))
            {
                await RestServices.loginPost(emailEntry.Text, passwordEntry.Text);
                await DisplayAlert("Success", "Login successful", "Ok");
            }
            else
            {
                await DisplayAlert("Error", "You must fill in both fields correctly!", "Ok");
            }
            if (!ServiceValidation.validateEmailAddress(emailEntry.Text))
            {
                emailErrorMessage.IsVisible = true;
            }
            if (!ServiceValidation.validatePassword(passwordEntry.Text))
            {
                passwordErrorMessage.IsVisible = true;
            }
        }
        else
        {
            await DisplayAlert("Error", "You are offline!", "Ok");
        }
		
    }

    private void emailEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        emailErrorMessage.IsVisible = false;
    }

    private void passwordEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        passwordErrorMessage.IsVisible = false;
    }

    private async void passwordForgotten_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ForgotPasswordPage());
    }
}