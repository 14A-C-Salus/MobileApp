using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class AuthorModel
    {
        public int id { get; set; }
        public decimal weight { get; set; }
        public decimal height { get; set; }
        public string birthDate { get; set; }
        public int gender { get; set; }
        public string genderString { get; set; }
        public decimal goalWeight { get; set; }
        public int hairIndex { get; set; }
        public int skinIndex { get; set; }
        public int eyesIndex { get; set; }
        public int mouthIndex { get; set; }
    }
}
