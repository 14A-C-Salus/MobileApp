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
        public ObservableCollection<RecipeModel> Recipe { get; set; }
        public NutritionPageViewModel()
        {
            Recipe = new ObservableCollection<RecipeModel>();
        }

        public void GetUserProfileFromViewModel()
        {
            
        }
    }
}
