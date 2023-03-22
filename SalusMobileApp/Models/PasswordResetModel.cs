using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Models
{
    public class PasswordResetModel
    {
        public string token { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public PasswordResetModel() { }
    }
}
