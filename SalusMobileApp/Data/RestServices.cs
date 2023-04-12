using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalusMobileApp.Models;
using SalusMobileApp.Pages.Login_Signup;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalusMobileApp.Data
{
    public class RestServices
    {
        static HttpClient _client;
        private static string _uri = "http://salushl-001-site1.dtempurl.com/api/";
        private static string _contentType = "application/json";
        private static string _registerUri = "Auth/register";
        private static string _loginUri = "Auth/login";
        //private static string _userDataUri = "Auth/get-auth?authId=";
        private static string _userProfileDataUri = "Auth/get-userprofile?authId=";
        private static string _forgotPasswordUri = "Auth/forgot-password?email=";
        private static string _passwordResetUri = "Auth/reset-password";
        private static string _createUserProfileUri = "UserProfile/create-profile";
        private static string _modifyUserProfileUri = "UserProfile/modify-profile";
        private static string _setProfilePictureUri = "UserProfile/set-profile-picture";
        private static string _writeCommentUri = "SocialMedia/write-comment";
        private static string _getCommentsByEmailUri = "SocialMedia/get-all-comment-by-authenticated-email";

        private static string _getFoodInformationByBarcodeUri = "https://world.openfoodfacts.org/api/v2/product/";
        public static async Task<bool> RegistrationPut(string username, string email, string password, string confirmPassword)
        {
            _client = new HttpClient();
            var registration = new RegistrationModel
            {
                username = username,
                email = email, 
                password = password,
                confirmPassword = confirmPassword
            };
            string requestUri = _uri + _registerUri;

            var json = JsonConvert.SerializeObject(registration);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            HttpResponseMessage response = await _client.PutAsync(requestUri, content);
            _client.Dispose();
            if(!response.IsSuccessStatusCode) 
            {
                return false;
            }
            return true;
        }
        public static async Task<bool> LoginPost(string email, string password)
        {
            _client = new HttpClient();
            var login = new LoginModel(email, password);

            string requestUri = _uri + _loginUri;
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PostAsync(requestUri, content);
            _client.Dispose();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            await ReturnUserId(response);
            return true;
        }

        private static async Task ReturnUserId(HttpResponseMessage response, string tokenValue = null)
        {
            if(tokenValue == null)
            {
                var returnedData = await response.Content.ReadAsStringAsync();
                App.jwtToken = returnedData;
            }
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(App.jwtToken);
            var tokenContentList = token.Claims.ToList();
            App.userId = tokenContentList[0].Value;
            var expires = long.Parse(tokenContentList[2].Value);
            App.tokenExpires = expires;
        }

        //public static async Task<bool> GetUserData(int id)
        //{
        //    _client = new HttpClient();

        //    string requestUri = _uri + _userDataUri + id.ToString();
        //    var response = await _client.GetAsync(requestUri);
        //    _client.Dispose();
        //    if(!response.IsSuccessStatusCode)
        //    {
        //        return false;
        //    }
        //    var returnedData = await response.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject(returnedData);
        //    string keyWord = "userProfile";
        //    App.userProfileExists = ReturnElementFromJson(result.ToString(), keyWord);
        //    return true;
        //}

        public static async Task<bool> GetUserProfileData(int id)
        {
            _client = new HttpClient();

            string requestUri = _uri + _userProfileDataUri + id.ToString();
            var response = await _client.GetAsync(requestUri);
            _client.Dispose();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = (JObject)JsonConvert.DeserializeObject(returnedData);
            var saveProfileData = new UserProfileModel
            {
                weight = int.Parse(result.SelectToken("weight").ToString()),
                height = int.Parse(result.SelectToken("height").ToString()),
                birthDate = result.SelectToken("birthDate").ToString(),
                gender = int.Parse(result.SelectToken("gender").ToString()),
                genderString = UserProfileModel.genderToString(int.Parse(result.SelectToken("gender").ToString())),
                goalWeight = int.Parse(result.SelectToken("goalWeight").ToString())
            };
            if(saveProfileData != null)
            {
                App.database.SaveLocalUserProfileData(saveProfileData);
            }
            return true;
        }

        public static async Task<bool> GetResetToken(string email)
        { 
            _client = new HttpClient();
            string emailUri = email.Replace("@", "%40");
            string requestUri = _uri + _forgotPasswordUri + emailUri;

            var json = JsonConvert.SerializeObject(email);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PatchAsync(requestUri, content);
            _client.Dispose();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            // Ez itt végleges változatban nem kell:
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject(returnedData);
            string keyWord = "passwordResetToken";
            App.passwordResetToken = ReturnElementFromJson(result.ToString(), keyWord);
            // --------------------------------------------------------------------------
            return true;
        }

        private static string ReturnElementFromJson(string source, string keyWord)
        {
            //var regex = new Regex(@"""" + keyWord + @"""\s*:\s*""([^""]+)""");
            var regex = new Regex(@"""" + keyWord + @""":\s*(\d+)");
            //var regex = new Regex(@"""passwordResetToken""\s*:\s*""([^""]+)""");
            //var regex = new Regex(regexText);
            return regex.Match(source).Groups[1].Value;
        }
        // Ez itt végleges változatban nem kell:
        public static async Task<bool> PasswordResetPatch(string token, string password, string confirmPassword)
        {
            _client = new HttpClient();
            var resetPassword = new PasswordResetModel 
            {
                token = token,
                password= password,
                confirmPassword= confirmPassword
            };
            string requestUri = _uri + _passwordResetUri;

            var json = JsonConvert.SerializeObject(resetPassword);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PatchAsync(requestUri, content);
            _client.Dispose();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            App.passwordResetToken = null;
            return true;
        }
        // ----------------------------------------------------------------------------------------------------------------
        public static async Task<bool> EditProfile(bool doesExist, int weight, int height, DateTime birthDate, int gender, string genderString, int goalWeight)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string isoBirthDateString = birthDate.ToString("o");
            var newUserProfile = new UserProfileModel(weight, height, isoBirthDateString, gender, goalWeight);
            string createRequestUri = _uri + _createUserProfileUri;
            string modifyRequestUri = _uri + _modifyUserProfileUri;

            var json = JsonConvert.SerializeObject(newUserProfile);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            newUserProfile = new UserProfileModel
            {
                weight = weight,
                height = height,
                birthDate = isoBirthDateString,
                genderString = genderString,
                goalWeight = goalWeight
            };
            HttpResponseMessage response;
            if(doesExist)
            {
                response = await _client.PatchAsync(modifyRequestUri, content);
                App.database.SaveLocalUserProfileData(newUserProfile);
            }
            else
            {
                response = await _client.PutAsync(createRequestUri, content);
                App.database.SaveLocalUserProfileData(newUserProfile);
            }
            
            _client.Dispose();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> SetProfilePicture(int hair, int skin, int eyes, int mouth)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            var profilePicture = new ProfilePictureModel
            {
                hairIndex = hair,
                skinIndex = skin,
                eyesIndex = eyes,
                mouthIndex = mouth
            };
            string requestUri = _uri + _setProfilePictureUri;

            var json = JsonConvert.SerializeObject(profilePicture);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PatchAsync(requestUri, content);
            _client.Dispose();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> SaveComment(string email, string body)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            var comment = new CommentModel(email, body);
            string requestUri = _uri + _writeCommentUri;

            var json = JsonConvert.SerializeObject(comment);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PutAsync(requestUri, content);
            _client.Dispose();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> GetCommentsByEmail()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _getCommentsByEmailUri;
            var response = await _client.GetAsync(requestUri);
            _client.Dispose();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> GetFoodInformationByBarcode(string barcode)
        {
            _client = new HttpClient();
            string requestUri = _getFoodInformationByBarcodeUri + barcode;
            var response = await _client.GetAsync(requestUri);
            _client.Dispose();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = (JObject)JsonConvert.DeserializeObject(returnedData);
            var kcal = int.Parse(result.SelectToken("product.nutriments.energy-kcal_100g").ToString());
            var protein = int.Parse(result.SelectToken("product.nutriments.proteins_100g").ToString());
            var carbohydrate = int.Parse(result.SelectToken("product.nutriments.carbohydrates_100g").ToString());
            var recipe = new RecipeModel(kcal, protein, carbohydrate);
            return true;
        }
    }
}
