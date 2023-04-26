using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class ComplexRecipeModel
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
        public int? method { get; set; }
        public int? oilId { get; set; }
        //public int[] ingredients { get; set; }
        public int[] ingredientIds { get; set; }
        public int[] ingredientPortionGramm { get; set; }
        public bool generateDescription { get; set; }
        public AuthorModel userProfile { get; set; }
        public int[] usersWhoLiked { get; set; }
        public int[] tags { get; set; }
        //public Last24h last24h { get; set; }
        //public class Last24h
        //{
        //    public int id { get; set; }
        //    public int gramm { get; set; }
        //    public int kcal { get; set; }
        //    public int protein { get; set; }
        //    public int fat { get; set; }
        //    public int carbohydrate { get; set; }
        //}
    }
}
