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
    public class GetRecipeByNameViewModel
    {
        public ObservableCollection<ComplexRecipeModel> Recipes { get; set; }
        public event EventHandler RecipeLoaded;
        public GetRecipeByNameViewModel()
        {
            Recipes = new ObservableCollection<ComplexRecipeModel>();
        }

        public async void GetRecipeByName(string name)
        {
            if (ServiceValidation.InternetConnectionValidator())
            {
                var data = await RestServices.GetRecipeByName(name);
                if (data != null)
                {
                    Recipes = new ObservableCollection<ComplexRecipeModel>(data);
                    if (RecipeLoaded != null)
                    {
                        RecipeLoaded(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
