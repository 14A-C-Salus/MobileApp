using SalusMobileApp.Data;

namespace SalusMobileApp.Pages.Login_Signup;

public partial class ResetPasswordPage : ContentPage
{
	public ResetPasswordPage()
	{
		InitializeComponent();
	}

    private async void resetPasswordButton_Clicked(object sender, EventArgs e)
    {
		if(ServiceValidation.InternetConnectionValidator())
		{
			if(ServiceValidation.ValidateConfirmedPassword(passwordEntry.Text, confirmPasswordEntry.Text)) 
			{
				var resetPasswordRequest = await RestServices.PasswordResetPatch(App.passwordResetToken.ToString(), passwordEntry.Text, confirmPasswordEntry.Text);
                if (resetPasswordRequest)
                {
                    await DisplayAlert("Success", "You have successfully reset your password!", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", "Internal server error, try again later!", "Ok");
                }
			}
			else
			{
				await DisplayAlert("Error", "Something went wrong, make sure the passwords match!", "Ok");
			}
		}
        else
        {
            await DisplayAlert("Error", "You are offline!", "Ok");
        }
    }

    private async void backToLogin_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
}