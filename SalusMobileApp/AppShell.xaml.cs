using SalusMobileApp.Pages;
using SalusMobileApp.Pages.Login_Signup;
using SalusMobileApp.Pages.MainMenu;
using SalusMobileApp.Pages.UserProfile;

namespace SalusMobileApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("registrationPage", typeof(RegistrationPage));
        Routing.RegisterRoute("loginPage", typeof(LoginPage));
		Routing.RegisterRoute("loginSignupPage", typeof(LoginSignupPage));
		Routing.RegisterRoute("forgotPasswordPage", typeof(ForgotPasswordPage));
		Routing.RegisterRoute("resetPasswordPage", typeof(ResetPasswordPage));
		Routing.RegisterRoute("editProfilePage", typeof(EditProfilePage));
		Routing.RegisterRoute("MainMenuPage", typeof(MainMenuPage));
    }
}
