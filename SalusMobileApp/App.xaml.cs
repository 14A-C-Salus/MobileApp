using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.Pages;
using System.Text;

namespace SalusMobileApp;

public partial class App : Application
{
	static LocalDatabase _database;
	public static string passwordResetToken;
	public static string jwtToken;
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
		database.DeleteLoginData();
		database.DeleteEncryptionData();
		LoginModel loggedIn = database.GetLoginData();
        UserProfileModel userProfile = database.GetLocalUserProfileData();
        _userProfile = userProfile;
        if (loggedIn != null)
		{
			if(ServiceValidation.InternetConnectionValidator())
			{
				var decryptedPassword = EncryptionModel.DecryptAsync(loggedIn.encryptedPassword);
				var login = new LoginPage();
				await login.CompleteLogin(loggedIn.email, decryptedPassword.Result, false, true);
            }
			else
			{
				// -----------------------------------------------------------------------------------------------------------------------------------------------
			}
		}
        
    }
}
