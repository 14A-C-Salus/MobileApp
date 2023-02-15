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
        if (checkRegistrationData(usernameEntry.Text, emailEntry.Text, passwordEntry.Text, confirmPasswordEntry.Text))
        {
            await RestServices.registrationPut(usernameEntry.Text, emailEntry.Text, passwordEntry.Text, confirmPasswordEntry.Text);
            await DisplayAlert("Success", "You successfully signed up to Salus! Check your e-mails and confirm your e-mail account.", "Ok");
        }
        else
        {
            await DisplayAlert("Error", "You must fill all fields!", "OK");
        }
    }

    private bool checkRegistrationData(string username, string email, string password, string confirmPassword)
    {
        if (username != null && email != null && password != null && confirmPassword != null && password == confirmPassword)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}