using Microsoft.Maui.Networking;
using SalusMobileApp.Data;
using SalusMobileApp.Models;
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
        if(ServiceValidation.InternetConnectionValidator()) 
        {
            if (ServiceValidation.ValidateLoginData(emailEntry.Text, passwordEntry.Text))
            {
                var loginRequest = await RestServices.LoginPost(emailEntry.Text, passwordEntry.Text);
                if(loginRequest)
                {
                    await DisplayAlert("Success", "Login successful", "Ok");
                    if(rememberPassword.IsChecked)
                    {
                        var login = new LoginModel
                        {
                            email = emailEntry.Text,
                            password = passwordEntry.Text,
                            jwtToken = App.jwtToken,
                        };
                        App.database.SaveLoginData(login);
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Internal server error, try again later!", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Error", "You must fill in both fields correctly!", "Ok");
            }
            if (!ServiceValidation.ValidateEmailAddress(emailEntry.Text))
            {
                emailErrorMessage.IsVisible = true;
            }
            if (!ServiceValidation.ValidatePassword(passwordEntry.Text))
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