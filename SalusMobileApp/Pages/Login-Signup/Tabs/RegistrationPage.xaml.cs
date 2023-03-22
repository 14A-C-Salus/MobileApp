using SalusMobileApp.Data;

namespace SalusMobileApp.Pages;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage()
	{
		InitializeComponent();
	}

    private async void registerButton_Clicked(object sender, EventArgs e)
    {
        if (ServiceValidation.InternetConnectionValidator())
        {
            if (ServiceValidation.CheckRegistrationData(usernameEntry.Text, emailEntry.Text, passwordEntry.Text, confirmPasswordEntry.Text))
            {
                var registrationRequest = await RestServices.RegistrationPut(usernameEntry.Text, emailEntry.Text, passwordEntry.Text, confirmPasswordEntry.Text);
                if (registrationRequest)
                {
                    await DisplayAlert("Success", "You successfully signed up to Salus! Check your e-mails and confirm your e-mail account.", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", "Internal server error, try again later!", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Error", "You must fill all fields!", "OK");
            }
        }
    }
}