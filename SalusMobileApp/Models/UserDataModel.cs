using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class UserDataModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string passwordSalt { get; set; }
        public bool isAdmin { get; set; }
        public string verificationToken { get; set; }
        public DateTime? date { get; set; }
        public object passwordResetToken { get; set; }
        public object resetTokenExpires { get; set; }
        public AuthorModel userProfile { get; set; }
    }
}
