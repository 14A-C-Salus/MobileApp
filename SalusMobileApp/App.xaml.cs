using SalusMobileApp.Data;
using SalusMobileApp.Models;

namespace SalusMobileApp;

public partial class App : Application
{
	static LocalDatabase _database;
	public static string passwordResetToken;
	public static string jwtToken;
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

    protected override void OnStart()
    {
		LoginModel loggedIn = database.GetLoginData();
        UserProfileModel userProfile = database.GetLocalUserProfileData();
		_userProfile = userProfile;
    }
}
