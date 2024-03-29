using SalusMobileApp.Data;

namespace SalusMobileApp.Pages;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage()
	{
		InitializeComponent();
	}

    private static string successfulSignup = "You successfully signed up to Salus! Check your e-mails and confirm your e-mail account.";
    private static string serverError = "Internal server error, try again later!";
    private static string fillAllFields = "You must fill all fields!";

    private async void registerButton_Clicked(object sender, EventArgs e)
    {
        registerButton.IsEnabled = false;
        if (ServiceValidation.InternetConnectionValidator())
        {
            if (ServiceValidation.CheckRegistrationData(usernameEntry.Text, emailEntry.Text, passwordEntry.Text, confirmPasswordEntry.Text))
            {
                var registrationRequest = await RestServices.RegistrationPut(usernameEntry.Text, emailEntry.Text, passwordEntry.Text, confirmPasswordEntry.Text);
                if (registrationRequest)
                {
                    await DisplayAlert("Success", successfulSignup, "Ok");
                }
                else
                {
                    await DisplayAlert("Error", serverError, "Ok");
                    registerButton.IsEnabled = true;
                }
            }
            else
            {
                await DisplayAlert("Error", fillAllFields, "OK");
                registerButton.IsEnabled = true;
            }
        }
        registerButton.IsEnabled = true;
    }

    private void passwordVisibleButton_Clicked(object sender, EventArgs e)
    {
        if (passwordEntry.IsPassword == true)
        {
            passwordEntry.IsPassword = false;
            confirmPasswordEntry.IsPassword = false;
            passwordVisibleButton.Source = "eyeactive.png";
        }
        else
        {
            passwordEntry.IsPassword = true; 
            confirmPasswordEntry.IsPassword = true;
            passwordVisibleButton.Source = "eyeinactive.png";
        }
    }
}