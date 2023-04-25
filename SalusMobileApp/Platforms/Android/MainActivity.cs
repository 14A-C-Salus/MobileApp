﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SalusMobileApp;

[Activity(//Theme = "@style/Maui.SplashTheme", 
    Theme = "@style/Maui.MainTheme.NoActionBar", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
