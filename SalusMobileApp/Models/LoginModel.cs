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
        public string userId { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public byte[] encryptedPassword { get; set; }
        public byte[] jwtToken { get; set; }
        public long tokenExpires { get; set; }
        
        public LoginModel(string email, string password) 
        {
            this.email = email;
            this.password = password;
        }
        public LoginModel() { }

        public static bool IsTokenExpired()
        {
            return App.tokenExpires - DateTimeOffset.Now.ToUnixTimeSeconds() <= 0;
        }
        
    }
}
