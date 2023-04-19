using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.Pages;
using SalusMobileApp.Pages.MainMenu;
using SalusMobileApp.Pages.Error;
using System.Text;
using SalusMobileApp.Pages.MainMenu.Tabs;
using SalusMobileApp.Pages.Login_Signup;
using SalusMobileApp.Pages.SplashScreen;

namespace SalusMobileApp;

public partial class App : Application
{
	static LocalDatabase _database;
	public static string passwordResetToken;
	public static string jwtToken;
	public static long tokenExpires;
	public static string userProfileExists;
	public static string userId;
	public static UserProfileModel _userProfile;
	public static RecipeModel mostRecentRecipe = new RecipeModel(0, 0, 0);
	public static LocalDatabase database
	{
		get
		{
			if (_database == null)
			{
				//_database = new LocalDatabase(Path.Combine(FileSystem.AppDataDirectory, "SalusMobileApp.db3"));
                _database = new LocalDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SalusMobileApp.db3"));
            }
			return _database;
		}

	}
    public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new SplashScreenPage());
    }

    protected async override void OnStart()
    {
        //database.DeleteLoginData();
        //database.DeleteEncryptionData();
        //database.DeleteLocalUserProfile();
        //OnApplicationStartupCustom();
        //LoginModel loggedIn = database.GetLoginData();
        //UserProfileModel userProfile = database.GetLocalUserProfileData();
        //_userProfile = userProfile;
        //var login = new LoginPage();
        //if (loggedIn != null)
        //{
        //    tokenExpires = loggedIn.tokenExpires;
        //    if (ServiceValidation.InternetConnectionValidator())
        //    {
        //        if (LoginModel.IsTokenExpired())
        //        {
        //            var decryptedPassword = EncryptionModel.DecryptAsync(loggedIn.encryptedPassword);
        //            await login.CompleteLogin(loggedIn.email, decryptedPassword.Result, false);
        //        }
        //        else
        //        {
        //            if (userProfile != null)
        //            {
        //                var decryptedJwtToken = EncryptionModel.DecryptAsync(database.GetLoginData().jwtToken);
        //                jwtToken = decryptedJwtToken.Result;
        //                //await Shell.Current.GoToAsync(nameof(MainMenuPage));
        //                //await login.NavigateToNextPage(true);
        //            }
        //            else
        //            {
        //                await Shell.Current.GoToAsync(nameof(ErrorPage));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        database.OfflineLogin();
        //    }
        //}
    }

    private async void OnApplicationStartupCustom()
    {
        LoginModel loggedIn = database.GetLoginData();
        UserProfileModel userProfile = database.GetLocalUserProfileData();
        _userProfile = userProfile;
        var login = new LoginPage();
        if (loggedIn != null)
        {
            tokenExpires = loggedIn.tokenExpires;
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
                        var decryptedJwtToken = EncryptionModel.DecryptAsync(database.GetLoginData().jwtToken);
                        jwtToken = decryptedJwtToken.Result;
                        //await Shell.Current.GoToAsync(nameof(MainMenuPage));
                        //await login.NavigateToNextPage(true);
                    }
                    else
                    {
                        await Shell.Current.GoToAsync(nameof(ErrorPage));
                    }
                }
            }
            else
            {
                database.OfflineLogin();
            }
        }
    }

    private void SetMainMenuAsMainpage()
    {
        MainPage = new NavigationPage(new MainMenuPage());
    }

    private void SetLoginSignupAsMainpage()
    {
        MainPage = new NavigationPage(new LoginSignupPage());
    }
}
