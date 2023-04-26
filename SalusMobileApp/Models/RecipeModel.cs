using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class RecipeModel
    {
        public int id { get; set; }
        public int fat { get; set; }
        public int kcal { get; set; }
        public int protein { get; set; }
        public int carbohydrate { get; set; }
        public int verified { get; set; }
        public int timeInMinutes { get; set; }
        public int? oilPortionMl { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public int method { get; set; }
        public int? oilId { get; set; }
        public string ingredientsString { get; set; }

        public RecipeModel() { }
        public RecipeModel(string name, int kcal, int protein, int fat, int carbohydrate)
        {
            this.name = name;
            this.kcal = kcal;
            this.protein = protein;
            this.fat = fat;
            this.carbohydrate = carbohydrate;
        }
    }
}
