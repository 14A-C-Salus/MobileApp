using Newtonsoft.Json.Linq;
using SalusMobileApp.Data;
using SalusMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.ViewModels
{
    public class NutritionPageViewModel
    {
        public ObservableCollection<ComplexLast24hModel> ConsumedMeals { get; set; }
        public event EventHandler MealsLoaded;
        public int DailyCalories;
        public NutritionPageViewModel()
        {
            ConsumedMeals = new ObservableCollection<ComplexLast24hModel>();
        }

        public async void GetConsumedMealsFromViewModelAsync(DateTime? date)
        {
            if (ServiceValidation.InternetConnectionValidator())
            {
                var data = await RestServices.GetLast24h(date);
                if (data != null)
                {
                    ConsumedMeals = new ObservableCollection<ComplexLast24hModel>(data);
                    if (MealsLoaded != null)
                    {
                        MealsLoaded(this, EventArgs.Empty);
                    }
                }
            }
        }
        public void DailyIdealCalorieIntake()
        {
            if (App._userProfile.genderString == "Male")
            {
                DailyCalories = Convert.ToInt32(Math.Ceiling(10 * App._userProfile.weight +(6.25 * App._userProfile.height) - (5 * (int.Parse(DateTime.Now.ToString("yyyyMMdd")) - int.Parse(App._userProfile.birthDate.Remove(App._userProfile.birthDate.Length - 7, 7)))) + 5));
            }
            else
            {
                DailyCalories = Convert.ToInt32(Math.Ceiling(10 * App._userProfile.weight + (6.25 * App._userProfile.height) - (5 * (int.Parse(DateTime.Now.ToString("yyyyMMdd")) - int.Parse(App._userProfile.birthDate.Remove(App._userProfile.birthDate.Length - 7, 7)))) - 161));
            }
        }
    }
}
