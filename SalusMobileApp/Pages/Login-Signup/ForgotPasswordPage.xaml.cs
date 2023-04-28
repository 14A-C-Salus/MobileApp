using SalusMobileApp.Data;

namespace SalusMobileApp.Pages.Login_Signup;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage()
	{
		InitializeComponent();
	}

    private async void forgottenPasswordButton_Clicked(object sender, EventArgs e)
    {
		if(ServiceValidation.InternetConnectionValidator())
		{
            if (ServiceValidation.ValidateEmailAddress(emailEntry.Text))
            {
                var forgottenPasswordRequest = await RestServices.GetResetToken(emailEntry.Text);
                if (forgottenPasswordRequest)
                {
                    await DisplayAlert("Success", "Check your emails to reset your password!", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", "Internal server error, try again later!", "Ok");
                }
            }
        }
		else
        {
            await DisplayAlert("Error", "You are offline!", "Ok");
        }
    }
}