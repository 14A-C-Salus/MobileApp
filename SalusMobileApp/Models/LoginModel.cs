using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class LoginModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string jwtToken { get; set; }
        
        public LoginModel(string email, string password) 
        {
            this.email = email;
            this.password = password;
        }
        public LoginModel() { }

        
    }
}
