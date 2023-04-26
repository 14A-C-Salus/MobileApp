using SalusMobileApp.Pages.AddFood;
using SalusMobileApp.ViewModels;

namespace SalusMobileApp.Pages.MainMenu.Tabs;

public partial class NutritionPage : ContentPage
{
    NutritionPageViewModel viewModel;
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
        await Navigation.PushAsync(new AddFoodPage());
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
            viewModel.MealsLoaded += (sender, e) =>
            todaysMealsListView.ItemsSource = viewModel.ConsumedMeals;
        }
    }

    private void decrementDateButton_Clicked(object sender, EventArgs e)
    {
        datePicker.Date = datePicker.Date.AddDays(-1);
        viewModel.GetConsumedMealsFromViewModelAsync(datePicker.Date);
        viewModel.MealsLoaded += (sender, e) =>
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
            viewModel.MealsLoaded += (sender, e) =>
            todaysMealsListView.ItemsSource = viewModel.ConsumedMeals;
        }
    }
}