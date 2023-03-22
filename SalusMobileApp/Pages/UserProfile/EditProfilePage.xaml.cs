using SalusMobileApp.Data;

namespace SalusMobileApp.Pages.UserProfile;

public partial class EditProfilePage : ContentPage
{
	public EditProfilePage()
	{
		InitializeComponent();
	}

    private async void confirmBtn_Clicked(object sender, EventArgs e)
    {
        if (ServiceValidation.InternetConnectionValidator())
        {
            if(ServiceValidation.UserProfileValidator(int.Parse(weightEntry.Text), int.Parse(heightEntry.Text), birthdatePicker.Date, genderPicker.SelectedItem.ToString(), int.Parse(goalWeightEntry.Text)))
            {
                bool profileExists;
                if(App._userProfile != null)
                {
                    profileExists = true;
                }
                else
                {
                    profileExists = false;
                }
                var userProfileEditRequest = await RestServices.EditProfile(profileExists, int.Parse(weightEntry.Text), int.Parse(heightEntry.Text), birthdatePicker.Date, ServiceValidation.GenderToNumber(genderPicker.SelectedItem.ToString()), int.Parse(goalWeightEntry.Text));
                if(userProfileEditRequest)
                {
                    await DisplayAlert("Success", "Your profile data has been successfully saved", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", "Something went wrong, please try again later", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid data", "Ok");
            }
            if (!ServiceValidation.ValidateUserWeight(int.Parse(weightEntry.Text)))
            {
                weightErrorMessage.IsVisible = true;
            }
            if (!ServiceValidation.ValidateUserHeight(int.Parse(heightEntry.Text)))
            {
                heightErrorMessage.IsVisible = true;
            }
            if (!ServiceValidation.ValidateUserBirthDate(birthdatePicker.Date))
            {
                weightErrorMessage.IsVisible = true;
            }
            if (!ServiceValidation.ValidateUserGender(genderPicker.SelectedItem.ToString()))
            {
                weightErrorMessage.IsVisible = true;
            }
            if (!ServiceValidation.ValidateUserGoalWeight(int.Parse(goalWeightEntry.Text)))
            {
                weightErrorMessage.IsVisible = true;
            }
        }
        else
        {
            await DisplayAlert("Error", "You are offline!", "Ok");
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
}