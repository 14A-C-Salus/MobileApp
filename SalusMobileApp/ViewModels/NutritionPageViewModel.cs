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
        public string DailyCalories;
        public int DailyCaloriesSum = 0;
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

                    DailyCaloriesSum = 0;
                    if (ConsumedMeals.Count > 0)
                    {
                        foreach (var meal in ConsumedMeals)
                        {
                            DailyCaloriesSum += meal.kcal;
                        }
                    }

                    if (MealsLoaded != null)
                    {
                        MealsLoaded(this, EventArgs.Empty);
                    }
                }
            }
        }
        public void DailyIdealCalorieIntake()
        {
            int birthdate = int.Parse(App._userProfile.birthDate.Remove(App._userProfile.birthDate.Length - 7, 7).Replace(".", string.Empty).Replace(" ", string.Empty));
            int today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            string age = (today - birthdate).ToString();
            int ageInt = int.Parse(age.Substring(0, age.Length - 4));
            int calories = Convert.ToInt32(Math.Ceiling(10 * App._userProfile.weight + (6.25 * App._userProfile.height) - (5 * ageInt)));
            if (App._userProfile.genderString == "Male")
            {
                calories += 5;
                DailyCalories = calories.ToString();
            }
            else
            {
                calories -= 161;
                DailyCalories = calories.ToString();
            }
        }
    }
}
