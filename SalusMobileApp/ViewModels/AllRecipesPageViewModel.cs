using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.ViewModels
{
    public class AllRecipesPageViewModel
    {
        public ObservableCollection<ComplexRecipeModel> Recipes { get; set; }
        public event EventHandler RecipeLoaded;
        public GetRecipeByNameViewModel()
        {
            Recipes = new ObservableCollection<ComplexRecipeModel>();
        }
    }
}
