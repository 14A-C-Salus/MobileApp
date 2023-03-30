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
        public static bool CheckRegistrationData(string username, string email, string password, string confirmPassword)
        {
            if (ValidateUsername(username)
                && email != null
                && ValidateConfirmedPassword(password, confirmPassword))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidateLoginData(string email, string password)
        {
            if(ValidateEmailAddress(email) && ValidatePassword(password))
            { 
                return true; 
            }
            return false;
        }

        public static bool ValidateUsername(string username)
        {
            if(username != null && username.Length >= 8 && username.Length <= 20)
            {
                return true;
            }
            return false;
        }

        public static bool ValidatePassword(string password)
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

        public static bool ValidateConfirmedPassword(string password, string confirmPassword)
        {
            if(ValidatePassword(password) && ValidatePassword(confirmPassword) && password == confirmPassword)
            {
                return true;
            }
            return false;
        }

        public static bool ValidateEmailAddress(string email)
        {
            if (email != null && IsValidEmail(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsValidEmail(string email)
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
        public static bool UserProfileValidator(int weight, int height, DateTime birthDate, string gender, int goalWeight)
        {
            if (ValidateUserWeight(weight) && ValidateUserHeight(height) && ValidateUserBirthDate(birthDate) && ValidateUserGender(gender) && ValidateUserGoalWeight(goalWeight))
            {
                return true;
            }
            return false;
        }

        public static bool ValidateUserWeight(int weight)
        {
            return IsBetween(weight, 20, 1000) && IsNothingNull(weight);
        }

        public static bool ValidateUserHeight(int height)
        {
            return IsBetween(height, 40, 250) && IsNothingNull(height);
        }

        public static bool ValidateUserBirthDate(DateTime birthDate)
        {
            return DateTime.Today.AddYears(-100) <= birthDate && DateTime.Today.AddYears(-12) >= birthDate && IsNothingNull(birthDate);
        }

        public static bool ValidateUserGender(object gender)
        {
            string genderString;
            if (gender != null)
            {
                genderString = gender.ToString();
            }
            else
            {
                genderString = "";
            }
            return IsBetween(GenderToNumber(genderString), 1, 3) && IsNothingNull(genderString);
        }

        public static bool ValidateUserGoalWeight(int goalWeight)
        {
            return IsBetween(goalWeight, 20, 1000) && IsNothingNull(goalWeight);
        }

        public static int GenderToNumber(string gender)
        {
            if (gender == "Male")
            {
                return 1;
            }
            if (gender == "Female")
            {
                return 2;
            }
            if (gender == "Other")
            {
                return 3;
            }
            return 0;
        }

        private static bool IsBetween(int number, int start, int end)
        {
            return number >= start && number <= end;
        }

        public static bool InternetConnectionValidator()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if(accessType == NetworkAccess.Internet) 
            {
                return true;
            }
            return false;
        }

        private static bool IsNothingNull(params object[] args)
        {
            foreach (object arg in args)
            {
                if(arg == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
