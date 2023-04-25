using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.Pages.Error;
using SalusMobileApp.Pages.Login_Signup;
using SalusMobileApp.Pages.MainMenu;

namespace SalusMobileApp.Pages.SplashScreen;

public partial class SplashScreenPage : ContentPage
{
    readonly Animation fadeIn;
	public SplashScreenPage()
	{
		InitializeComponent();
        fadeIn = new Animation(v => logoImage.Opacity = v, 0, 1, Easing.Linear);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        fadeIn.Commit(this, "fadeIn", length: 5000, easing: Easing.Linear, finished: async (d, b) =>
        {
            LoginModel loggedIn = App.database.GetLoginData();
            UserProfileModel userProfile = App.database.GetLocalUserProfileData();
            var login = new LoginPage();
            if (loggedIn != null)
            {
                App.userId = loggedIn.userId;
                App._userProfile = userProfile;
                App.tokenExpires = loggedIn.tokenExpires;
                if (ServiceValidation.InternetConnectionValidator())
                {
                    if (LoginModel.IsTokenExpired())
                    {
                        var decryptedPassword = EncryptionModel.DecryptAsync(loggedIn.encryptedPassword);
                        await login.CompleteLogin(loggedIn.email, decryptedPassword.Result, false);
                    }
                    else
                    {
                        if (userProfile != null)
                        {
                            var decryptedJwtToken = EncryptionModel.DecryptAsync(App.database.GetLoginData().jwtToken);
                            App.jwtToken = decryptedJwtToken.Result;
                            await Navigation.PushAsync(new MainMenuPage());
                        }
                        else
                        {
                            await Navigation.PushAsync(new ErrorPage());
                        }
                    }
                }
                else
                {
                    App.database.OfflineLogin();
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginSignupPage());
            }
        });
    }
}