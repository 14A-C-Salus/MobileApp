using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class SimpleRecipeModel
    {
        public string name { get; set; }
        public int fat { get; set; }
        public int protein { get; set; }
        public int kcal { get; set; }
        public int carbohydrate { get; set; }
    }
}
