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
        public ObservableCollection<Recipe> ConsumedMeals { get; set; }
        public event EventHandler MealsLoaded;
        public NutritionPageViewModel()
        {
            ConsumedMeals = new ObservableCollection<Recipe>();
        }

        public async void GetConsumedMealsFromViewModelAsync(DateTime? date)
        {
            if (ServiceValidation.InternetConnectionValidator())
            {
                var data = await RestServices.GetLast24h(date);
                //var results = data.SelectToken("$.[*].recipes[*].name");
                ObservableCollection<Recipe> meals = new ObservableCollection<Recipe>();
                var results = data.SelectToken("$.[*].recipes");
                if (results != null)
                {
                    meals = results.ToObject<ObservableCollection<Recipe>>();
                }
                if (data != null)
                {
                    ConsumedMeals = new ObservableCollection<Recipe>(meals);
                    if (MealsLoaded != null)
                    {
                        MealsLoaded(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
