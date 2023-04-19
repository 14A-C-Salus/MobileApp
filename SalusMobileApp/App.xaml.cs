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
				// This is a faster method but it doesn't work for some reason. I will try to make it work if everything else is done.
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
}
