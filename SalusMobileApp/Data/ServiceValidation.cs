using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace SalusMobileApp.Data
{
    public class ServiceValidation
    {
        public static bool checkRegistrationData(string username, string email, string password, string confirmPassword)
        {
            if (username != null
                && email != null
                && password != null
                && confirmPassword != null
                && password == confirmPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool validateLoginData(string email, string password)
        {
            if(validateEmailAddress(email) && validatePassword(password))
            { 
                return true; 
            }
            return false;
        }

        public static bool validateUsername(string username)
        {
            if(username != null && username.Length >= 8 && username.Length <= 20)
            {
                return true;
            }
            return false;
        }

        public static bool validatePassword(string password)
        {
            if (password != null && password.Length >= 8 && password.Length <= 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool validateEmailAddress(string email)
        {
            if (email != null && isValidEmail(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool isValidEmail(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
    }
}
