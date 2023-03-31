using SalusMobileApp.Data;

namespace SalusMobileApp.Pages.UserProfile;

public partial class EditProfilePage : ContentPage
{
	public EditProfilePage()
	{
		InitializeComponent();
	}

    private static string successfullySaved = "Your profile data has been successfully saved";
    private static string serverError = "Something went wrong, please try again later";
    private static string filledInError = "Invalid data, all fields must be filled in";
    private static string offlineError = "You are offline!";

    private async void confirmBtn_Clicked(object sender, EventArgs e)
    {
        if (ServiceValidation.InternetConnectionValidator())
        {
            if (ServiceValidation.UserProfileValidator(CanBeConvertedToInt(weightEntry.Text), CanBeConvertedToInt(heightEntry.Text), birthdatePicker.Date, Convert.ToString(genderPicker.SelectedItem), CanBeConvertedToInt(goalWeightEntry.Text)))
            {
                var userProfileEditRequest = await SendEditRequest();
                if (userProfileEditRequest)
                {
                    await DisplayAlert("Success", successfullySaved, "Ok");
                }
                else
                {
                    await DisplayAlert("Error", serverError, "Ok");
                }
            }
            else
            {
                await DisplayAlert("Error", filledInError, "Ok");
            }
            ErrorMessageIfFilledInIncorrectly();
        }
        else
        {
            await DisplayAlert("Error", offlineError, "Ok");
        }
    }

    private void weightEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        weightErrorMessage.IsVisible = false;
    }

    private void heightEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        heightErrorMessage.IsVisible = false;
    }

    private void birthdatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        birthdateErrorMessage.IsVisible = false;
    }

    private void genderPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        genderErrorMessage.IsVisible = false;
    }

    private void goalWeightEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        goalWeightErrorMessage.IsVisible = false;
    }

    private int CanBeConvertedToInt(string value)
    {
        if(int.TryParse(value, out int result))
        {
            return result;
        }
        else
        {
            return 0;
        }
    }

    private async Task<bool> SendEditRequest()
    {
        bool profileExists;
        if (App._userProfile != null)
        {
            profileExists = true;
        }
        else
        {
            profileExists = false;
        }
        return await RestServices.EditProfile(profileExists, CanBeConvertedToInt(weightEntry.Text), CanBeConvertedToInt(heightEntry.Text), birthdatePicker.Date, ServiceValidation.GenderToNumber(genderPicker.SelectedItem.ToString()), genderPicker.SelectedItem.ToString(), CanBeConvertedToInt(goalWeightEntry.Text));
    }

    private void ErrorMessageIfFilledInIncorrectly()
    {
        if (!ServiceValidation.ValidateUserWeight(CanBeConvertedToInt(weightEntry.Text)))
        {
            weightErrorMessage.IsVisible = true;
        }
        if (!ServiceValidation.ValidateUserHeight(CanBeConvertedToInt(heightEntry.Text)))
        {
            heightErrorMessage.IsVisible = true;
        }
        if (!ServiceValidation.ValidateUserBirthDate(birthdatePicker.Date))
        {
            birthdateErrorMessage.IsVisible = true;
        }
        if (!ServiceValidation.ValidateUserGender(genderPicker.SelectedItem))
        {
            genderErrorMessage.IsVisible = true;
        }
        if (!ServiceValidation.ValidateUserGoalWeight(CanBeConvertedToInt(goalWeightEntry.Text)))
        {
            goalWeightErrorMessage.IsVisible = true;
        }
    }
}