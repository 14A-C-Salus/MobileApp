using SalusMobileApp.Pages;
using SalusMobileApp.Pages.Login_Signup;
using SalusMobileApp.Pages.MainMenu;
using SalusMobileApp.Pages.UserProfile;
using SalusMobileApp.Pages.Error;
using SalusMobileApp.Pages.MainMenu.Tabs;

namespace SalusMobileApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(LoginSignupPage), typeof(LoginSignupPage));
		Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
		Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
		Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
		Routing.RegisterRoute(nameof(MainMenuPage), typeof(MainMenuPage));
		Routing.RegisterRoute(nameof(ErrorPage), typeof(ErrorPage));
    }
}
