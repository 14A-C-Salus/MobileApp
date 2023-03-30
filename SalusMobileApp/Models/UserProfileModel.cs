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
        public int goalWeight { get; set; }

        public UserProfileModel() { }
    }
}
