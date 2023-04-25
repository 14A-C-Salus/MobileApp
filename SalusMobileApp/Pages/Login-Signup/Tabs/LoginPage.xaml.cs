using Microsoft.Maui.Networking;
using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.Pages.Login_Signup;
using SalusMobileApp.Pages.MainMenu;
using SalusMobileApp.Pages.MainMenu.Tabs;
using SalusMobileApp.Pages.UserProfile;
using System.Formats.Asn1;
using System.Text;

namespace SalusMobileApp.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private static string incorrectFieldsErrorMessage = "You must fill in both fields correctly!";
    private static string loginErrorMessage = "Something went wrong, please check whether you filled out both fields correctly!";

    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        loginButton.IsEnabled = false;
        passwordForgotten.IsEnabled = false;
        if(ServiceValidation.InternetConnectionValidator()) 
        {
            if (ServiceValidation.ValidateLoginData(emailEntry.Text, passwordEntry.Text))
            {
                var login = await CompleteLogin(emailEntry.Text, passwordEntry.Text, rememberPassword.IsChecked);
                if(!login)
                {
                    await DisplayAlert("Error", loginErrorMessage, "Ok");
                }
            }
            else
            {
                await DisplayAlert("Error", incorrectFieldsErrorMessage, "Ok");
                loginButton.IsEnabled = true;
                passwordForgotten.IsEnabled = true;
            }
            ErrorMessageIfFilledInIncorrectly();
        }
        else
        {
            await DisplayAlert("Error", "You are offline!", "Ok");
        }
        loginButton.IsEnabled = true;
        passwordForgotten.IsEnabled = true;
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
    // Methods:
    // -------------------------------------------------------------------------------
    // -------------------------------------------------------------------------------
    public async Task<bool> CompleteLogin(string email, string password, bool isChecked)
    {
        var loginRequest = await RestServices.LoginPost(email, password);
        if (loginRequest)
        {
            var thisUserProfile = await RestServices.GetUserProfileData(int.Parse(App.userId));
            await RememberPassword(isChecked, email, password);
            await NavigateToNextPage(thisUserProfile);
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task RememberPassword(bool isChecked, string email, string password)
    {
        if (isChecked)
        {
            var encryptedPassword = await EncryptionModel.EncryptAsync(password);
            Thread.Sleep(10);
            var encryptedJwtToken = await EncryptionModel.EncryptAsync(App.jwtToken, false);
            var login = new LoginModel
            {
                email = email,
                encryptedPassword = encryptedPassword,
                jwtToken = encryptedJwtToken,
                userId = App.userId,
                tokenExpires = App.tokenExpires,
            };
            App.database.SaveLoginData(login);
        }
        else
        {
            App.database.DeleteLoginData();
        }
    }

    public async Task NavigateToNextPage(bool doesProfileExist)
    {
        if (!doesProfileExist)
        {
            //await Shell.Current.GoToAsync(nameof(EditProfilePage));
            await Navigation.PushAsync(new EditProfilePage());
        }
        else
        {
            UserProfileModel userProfile = App.database.GetLocalUserProfileData();
            App._userProfile = userProfile;
            //await Shell.Current.GoToAsync(nameof(MainMenuPage));
            await Navigation.PushAsync(new MainMenuPage());
        }
    }

    private void ErrorMessageIfFilledInIncorrectly()
    {
        if (!ServiceValidation.ValidateEmailAddress(emailEntry.Text))
        {
            emailErrorMessage.IsVisible = true;
        }
        if (!ServiceValidation.ValidatePassword(passwordEntry.Text))
        {
            passwordErrorMessage.IsVisible = true;
        }
    }

    private void passwordVisibleButton_Clicked(object sender, EventArgs e)
    {
        if(passwordEntry.IsPassword == true)
        {
            passwordEntry.IsPassword = false;
            passwordVisibleButton.Source = "eyeactive.png";
        }
        else
        {
            passwordEntry.IsPassword = true;
            passwordVisibleButton.Source = "eyeinactive.png";
        }
    }
}