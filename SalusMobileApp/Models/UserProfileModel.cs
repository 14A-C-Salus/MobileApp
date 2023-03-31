using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class UserProfileModel
    {
        public int weight { get; set; }
        public int height { get; set; }
        public string birthDate { get; set; }
        public int gender { get; set; }
        public string genderString { get; set; }
        public int goalWeight { get; set; }

        public UserProfileModel() { }

        public UserProfileModel(int weightIn, int heightIn, string birthdateIn, int genderIn, int goalweightIn)
        {
            weight = weightIn;
            height = heightIn;
            birthDate = birthdateIn;
            gender = genderIn;
            goalWeight = goalweightIn;
        }

        public static string genderToString(int gender)
        {
            switch (gender)
            {
                case 1: return "Male";
                case 2: return "Female";
                case 3: return "Other";
                default:return null;
            }
        }
    }
}
