using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.Pages;
using SalusMobileApp.Pages.MainMenu;
using SalusMobileApp.Pages.Error;
using System.Text;
using SalusMobileApp.Pages.MainMenu.Tabs;

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
	public static LocalDatabase database
	{
		get
		{
			if (_database == null)
			{
				_database = new LocalDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SalusMobileApp.db3"));
			}
			return _database;
		}

	}
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override async void OnStart()
    {
		//database.DeleteLoginData();
		//database.DeleteEncryptionData();
		//database.DeleteLocalUserProfile();

		LoginModel loggedIn = database.GetLoginData();
        UserProfileModel userProfile = database.GetLocalUserProfileData();
        _userProfile = userProfile;
        if (loggedIn != null)
		{
            tokenExpires = loggedIn.tokenExpires;
            if (ServiceValidation.InternetConnectionValidator())
			{
				if(LoginModel.IsTokenExpired())
				{
                    var decryptedPassword = EncryptionModel.DecryptAsync(loggedIn.encryptedPassword);
                    var login = new LoginPage();
                    await login.CompleteLogin(loggedIn.email, decryptedPassword.Result, false, true);
                }
				else
				{
					if(userProfile != null)
					{
						var decryptedJwtToken = EncryptionModel.DecryptAsync(database.GetLoginData().jwtToken);
						jwtToken = decryptedJwtToken.Result;
                        await Shell.Current.GoToAsync(nameof(MainMenuPage));
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
}
