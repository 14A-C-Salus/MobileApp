using Newtonsoft.Json;
using SalusMobileApp.Models;
using SalusMobileApp.Pages.Login_Signup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.Data
{
    public class RestServices
    {
        static HttpClient _client;
        public static string _uri = "http://salusprojekt-001-site1.dtempurl.com/api/";
        public static string _contentType = "application/json";
        public static string _registerUri = "Auth/register";
        public static string _loginUri = "Auth/login";
        public static string _passwordResetUri = "Auth/forgot-password?email=";
        public static async Task<bool> registrationPut(string username, string email, string password, string confirmPassword)
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
            if(!response.IsSuccessStatusCode) 
            {
                return false;
            }
            return true;
        }
        public static async Task<bool> loginPost(string email, string password)
        {
            _client = new HttpClient();
            var login = new LoginModel
            {
                email = email,
                password = password
            };

            string requestUri = _uri + _loginUri;
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PostAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            return true;
        }

        public static async Task<bool> passwordResetPatch(string email)
        { 
            _client = new HttpClient();
            string emailUri = email.Replace("@", "%40");
            string requestUri = _uri + _passwordResetUri + emailUri;

            var json = JsonConvert.SerializeObject(email);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PatchAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject(returnedData);
            return true;
        }
    }
}
