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
		if(ServiceValidation.validateEmailAddress(emailEntry.Text)) 
		{
			await RestServices.passwordResetPatch(emailEntry.Text);
            await Navigation.PushAsync(new ResetPasswordPage());
        }
    }
}