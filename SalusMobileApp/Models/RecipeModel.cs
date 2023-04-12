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
        public int gramm { get; set; }
        public int kcal { get; set; }
        public int protein { get; set; }
        public int carbohydrate { get; set; }
        public int verified { get; set; }
        public int timeInMinute { get; set; }
        public int oilPortionMl { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public int method { get; set; }
        public int oilId { get; set; }
        public int last24hid { get; set; }

        public RecipeModel() { }
        public RecipeModel(int kcal, int protein, int carbohydrate)
        {

        }
    }
}
