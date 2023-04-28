using SalusMobileApp.Data;
using SalusMobileApp.Models;
using SalusMobileApp.Pages.AddFood;
using SalusMobileApp.Pages.MainMenu.MealPages;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.Tabs;

public partial class NutritionPage : ContentPage
{
    NutritionPageViewModel viewModel;
    private string noInternetErrorMessage = "This action requires internet connection.";
	public NutritionPage()
	{
		InitializeComponent();
        viewModel = new NutritionPageViewModel();
        BindingContext = viewModel;
        viewModel.GetConsumedMealsFromViewModelAsync(DateTime.Today);
        viewModel.MealsLoaded += (sender, e) =>
        todaysMealsListView.ItemsSource = viewModel.ConsumedMeals;
	}

    private async void addNewButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddMealsPages());
    }

    private void datePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        if(datePicker.Date >  DateTime.Today)
        {
            DisplayAlert("Error", "You can't select future dates", "Ok");
            datePicker.Date = DateTime.Today;
        }
        else
        {
            viewModel.GetConsumedMealsFromViewModelAsync(datePicker.Date);
            todaysMealsListView.ItemsSource = viewModel.ConsumedMeals;
        }
    }

    private void decrementDateButton_Clicked(object sender, EventArgs e)
    {
        datePicker.Date = datePicker.Date.AddDays(-1);
        viewModel.GetConsumedMealsFromViewModelAsync(datePicker.Date);
        todaysMealsListView.ItemsSource = viewModel.ConsumedMeals;
    }

    private void incrementDateButton_Clicked(object sender, EventArgs e)
    {
        datePicker.Date = datePicker.Date.AddDays(1);
        if (datePicker.Date > DateTime.Today)
        {
            DisplayAlert("Error", "You can't select future dates", "Ok");
            datePicker.Date = DateTime.Today;
        }
        else
        {
            
            viewModel.GetConsumedMealsFromViewModelAsync(datePicker.Date);
            todaysMealsListView.ItemsSource = viewModel.ConsumedMeals;
        }
    }

    private async void todaysMealsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selected = (ComplexLast24hModel)todaysMealsListView.SelectedItem;
        if(selected != null)
        {
            string choice = await DisplayActionSheet("What do you want to do with this meal?", "Cancel", "Delete", "Half portion", "Third portion", "Quarter portion", "Double portion");
            switch (choice)
            {
                case "Cancel":
                    break;
                case "Delete":
                    if(ServiceValidation.InternetConnectionValidator())
                    {
                        var delete = await RestServices.DeleteLast24hById(selected.id);
                        if (delete)
                        {
                            viewModel.GetConsumedMealsFromViewModelAsync(DateTime.Today);
                            viewModel.MealsLoaded += (sender, e) =>
                            todaysMealsListView.ItemsSource = viewModel.ConsumedMeals;
                        }
                        else
                        {
                            await DisplayAlert("Error", "Couldn't delete the selected meal.", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", noInternetErrorMessage, "Ok");
                    }
                    break;
                default:
                    if(ServiceValidation.InternetConnectionValidator())
                    {
                        await RestServices.PortionModifier(selected.id, choice);
                        todaysMealsListView.SelectedItem = null;
                    }
                    else
                    {
                        await DisplayAlert("Error", noInternetErrorMessage, "Ok");
                    }
                    break;
            }
        }
        todaysMealsListView.SelectedItem = null;
    }

    private void reloadButton_Clicked(object sender, EventArgs e)
    {
        viewModel.GetConsumedMealsFromViewModelAsync(DateTime.Today);
        viewModel.MealsLoaded += (sender, e) =>
        todaysMealsListView.ItemsSource = viewModel.ConsumedMeals;
    }
}