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
    public class GetRecipeByAuthIdViewModel
    {
        public ObservableCollection<ComplexRecipeModel> Recipes { get; set; }
        public ObservableCollection<RecipeModel> FavouriteRecipes { get; set; }
        public event EventHandler RecipeLoaded;
        public GetRecipeByAuthIdViewModel()
        {
            Recipes = new ObservableCollection<ComplexRecipeModel>();
        }

        public async void GetRecipeById(int id)
        {
            if (ServiceValidation.InternetConnectionValidator())
            {
                var data = await RestServices.GetAllRecipeByAuthIdAsList(id);
                if (data != null)
                {
                    Recipes = new ObservableCollection<ComplexRecipeModel>(data);
                    //if(Recipes != null)
                    //{
                    //    return true;
                    //}
                    if (RecipeLoaded != null)
                    {
                        RecipeLoaded(this, EventArgs.Empty);
                    }
                }
            }
        }

        public void GetRecipeFromLocalDatabase()
        {
            var data = App.database.GetFavouriteRecipes();
            if (data != null)
            {
                FavouriteRecipes = new ObservableCollection<RecipeModel>(data);
                if (RecipeLoaded != null)
                {
                    RecipeLoaded(this, EventArgs.Empty);
                }
            }
        }
    }
}
