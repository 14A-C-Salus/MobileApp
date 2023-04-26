using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class ComplexLast24hModel
    {
        public int id { get; set; }
        public int gramm { get; set; }
        public int kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
        public int liquidInDl { get; set; }
        public DateTime time { get; set; }
        public int userProfileId { get; set; }
        public Recipe[] recipes { get; set; }
    }

    public class Recipe
    {
        public int id { get; set; }
        public int gramm { get; set; }
        public int kcal { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrate { get; set; }
        public bool verifeid { get; set; }
        public int timeInMinute { get; set; }
        public object oilPortionMl { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public int method { get; set; }
        public Userprofile userProfile { get; set; }
        public object ingredients { get; set; }
        public object usersWhoLiked { get; set; }
        public object oilId { get; set; }
        public object oil { get; set; }
        public object[] recipes { get; set; }
        public object[] tags { get; set; }
    }

    public class Userprofile
    {
        public int id { get; set; }
        public int weight { get; set; }
        public int height { get; set; }
        public DateTime birthDate { get; set; }
        public int gender { get; set; }
        public int goalWeight { get; set; }
        public int hairIndex { get; set; }
        public int skinIndex { get; set; }
        public int eyesIndex { get; set; }
        public int mouthIndex { get; set; }
    }








}
