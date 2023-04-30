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
        private readonly static HttpClient _client = new HttpClient();
        private static string _uri = "https://salus.azurewebsites.net/api/";
        
        //private static string _uri = "https://localhost:7138/api/";

        private static string _contentType = "application/json";
        private static string _registerUri = "Auth/register";
        private static string _loginUri = "Auth/login";
        private static string _userProfileDataUri = "Auth/get-userprofile?authId=";
        private static string _userDataUri = "Auth/get-auth?authId=";
        private static string _forgotPasswordUri = "Auth/forgot-password?email=";
        private static string _passwordResetUri = "Auth/reset-password";

        private static string _createUserProfileUri = "UserProfile/create-profile";
        private static string _modifyUserProfileUri = "UserProfile/modify-profile";
        private static string _setProfilePictureUri = "UserProfile/set-profile-picture";
        private static string _getUserProfilesByNameUri = "UserProfile/get-userprofiles-by-name?name=";

        private static string _getAllLast24hUri = "Last24h/get-all";
        private static string _getLast24hByDateUri = "Last24h/get-all?date=";
        private static string _postLast24hUri = "Last24h/add-new-recipe";
        private static string _halfPortionUri = "Last24h/half-portion?id=";
        private static string _thirdPortionUri = "Last24h/third-portion?id=";
        private static string _quarterPortionUri = "Last24h/quarter-portion?id=";
        private static string _doublePortionUri = "Last24h/double-portion?id=";
        private static string _deleteLast24ByIdUri = "Last24h/delete?id=";

        private static string _getRecipeByNameUri = "Recipe/get-recipes-by-name?name=";
        private static string _getAllRecipeByAuthIdUri = "Recipe/get-all-recipe-by-user-id?userProfileId=";
        private static string _createNewFoodSimpleUri = "Recipe/create-simple";
        private static string _createNewFoodUri = "Recipe/create";
        private static string _deleteFoodUri = "Recipe/delete?id=";
        private static string _likeUnlikeRecipeUri = "Recipe/like-unlike?recipeId=";

        private static string _writeCommentUri = "SocialMedia/write-comment";
        private static string _getCommentsByEmailUri = "SocialMedia/get-all-comment-by-authenticated-email";
        private static string _getCommentsByIdUri = "SocialMedia/get-all-comment-by-userprofile-id?userprofileId=";

        private static string _getFoodInformationByBarcodeUri = "https://world.openfoodfacts.org/api/v2/product/";
        // ------------------------------------ User START ------------------------------------------------------------------------
        public static async Task<bool> RegistrationPut(string username, string email, string password, string confirmPassword)
        {
             
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
        public static async Task<bool> LoginPost(string email, string password)
        {
             
            var login = new LoginModel(email, password);

            string requestUri = _uri + _loginUri;
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PostAsync(requestUri, content);
             
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
        public static async Task<bool> GetUserProfileData(int id)
        {
             

            string requestUri = _uri + _userProfileDataUri + id.ToString();
            var response = await _client.GetAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = (JObject)JsonConvert.DeserializeObject(returnedData);
            var saveProfileData = new UserProfileModel
            {
                id = int.Parse(result.SelectToken("id").ToString()),
                weight = int.Parse(result.SelectToken("weight").ToString()),
                height = int.Parse(result.SelectToken("height").ToString()),
                birthDate = result.SelectToken("birthDate").ToString(),
                gender = int.Parse(result.SelectToken("gender").ToString()),
                genderString = UserProfileModel.genderToString(int.Parse(result.SelectToken("gender").ToString())),
                goalWeight = int.Parse(result.SelectToken("goalWeight").ToString())
            };
            if (saveProfileData != null && App.database.GetLoginData != null)
            {
                App.database.SaveLocalUserProfileData(saveProfileData);
            }
            App._userProfile = saveProfileData;
            return true;
        }

        public static async Task<object> GetProfileData(int id)
        {
             

            string requestUri = _uri + _userDataUri + id.ToString();
            var response = await _client.GetAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = (JObject)JsonConvert.DeserializeObject(returnedData);
            return result;
        }

        public static async Task<UserProfileModel> GetUserProfileDataAsObject(int id)
        {
             

            string requestUri = _uri + _userProfileDataUri + id.ToString();
            var response = await _client.GetAsync(requestUri);
             
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = (JObject)JsonConvert.DeserializeObject(returnedData);
            var profileData = new UserProfileModel
            {
                weight = int.Parse(result.SelectToken("weight").ToString()),
                height = int.Parse(result.SelectToken("height").ToString()),
                birthDate = result.SelectToken("birthDate").ToString(),
                gender = int.Parse(result.SelectToken("gender").ToString()),
                genderString = UserProfileModel.genderToString(int.Parse(result.SelectToken("gender").ToString())),
                goalWeight = int.Parse(result.SelectToken("goalWeight").ToString())
            };
            return profileData;
        }

        public static async Task<bool> GetResetToken(string email)
        { 
             
            string emailUri = email.Replace("@", "%40");
            string requestUri = _uri + _forgotPasswordUri + emailUri;

            var json = JsonConvert.SerializeObject(email);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PatchAsync(requestUri, content);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
        public static async Task<bool> EditProfile(bool doesExist, int weight, int height, DateTime birthDate, int gender, string genderString, int goalWeight)
        {
             
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
                if(App.database.GetLoginData() != null)
                {
                    App.database.SaveLocalUserProfileData(newUserProfile);
                }
                
            }
            else
            {
                response = await _client.PutAsync(createRequestUri, content);
                if (App.database.GetLoginData() != null)
                {
                    App.database.SaveLocalUserProfileData(newUserProfile);
                }
            }
            
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> SetProfilePicture(int hair, int skin, int eyes, int mouth)
        {
             
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
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<List<UserDataModel>> GetUserProfileByName(string name)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _getUserProfilesByNameUri + name;
            var response = await _client.GetAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<UserDataModel>>(returnedData);
            return result;
        }
        // ------------------------------------ User END ------------------------------------------------------------------------
        // ------------------------------------ Comment START ------------------------------------------------------------------------
        public static async Task<bool> SaveComment(string email, string body)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            var comment = new CommentModel(email, body);
            string requestUri = _uri + _writeCommentUri;

            var json = JsonConvert.SerializeObject(comment);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PutAsync(requestUri, content);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> GetCommentsByEmail()
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _getCommentsByEmailUri;
            var response = await _client.GetAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> GetCommentsById(int id)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _getCommentsByIdUri + id.ToString();
            var response = await _client.GetAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
        // ------------------------------------ Comment END ------------------------------------------------------------------------

        // ------------------------------------ Recipe START ------------------------------------------------------------------------

        public static async Task<bool> GetFoodInformationByBarcode(string barcode)
        {
             
            string requestUri = _getFoodInformationByBarcodeUri + barcode;
            var response = await _client.GetAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            GetDataOutOfBarcodeResponse(returnedData);
            return true;
        }

        private static void GetDataOutOfBarcodeResponse(string returnedData)
        {
            var result = (JObject)JsonConvert.DeserializeObject(returnedData);
            var name = result.SelectToken("product.ecoscore_data.agribalyse.name_en").ToString();
            var kj = ReturnIntFromBarcodeToken(result.SelectToken("product.nutriments.energy-kj_100g").ToString());
            int kcal = Convert.ToInt32(Math.Ceiling((decimal)kj / 4));
            var protein = ReturnIntFromBarcodeToken(result.SelectToken("product.nutriments.proteins_100g").ToString());
            var carbohydrate = ReturnIntFromBarcodeToken(result.SelectToken("product.nutriments.carbohydrates_100g").ToString());
            var fat = ReturnIntFromBarcodeToken(result.SelectToken("product.nutriments.fat_100g").ToString());
            App.mostRecentRecipe = new RecipeModel(name, kcal, protein, fat, carbohydrate);
        }

        private static int ReturnIntFromBarcodeToken(string data)
        {
            decimal dataInDecimal = Convert.ToDecimal(data);
            return Convert.ToInt32(Math.Ceiling(dataInDecimal));
        }

        public static async Task<bool> CreateRecipeSimple(string name, int kcal, int protein, int fat, int carbohydrate)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            var recipe = new RecipeModel(name, kcal, protein, fat, carbohydrate);
            string requestUri = _uri + _createNewFoodSimpleUri;
            
            var json = JsonConvert.SerializeObject(recipe);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PutAsync(requestUri, content);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            return true;
        }

        public static async Task<bool> CreateRecipe(int[] igredientIds, int[] ingredientPortionGram, int method, int oilId, int oilPortionMl, int timeInMinutes, string name, bool generateDescription, string description)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            var descriptionValue = " ";
            if(description != null)
            {
                descriptionValue = description;
            }
            var recipe = new ComplexRecipeModel
            {
                ingredientIds = igredientIds,
                ingredientPortionGramm = ingredientPortionGram,
                method = method,
                oilId = oilId,
                oilPortionMl = oilPortionMl,
                timeInMinutes = timeInMinutes,
                name = name,
                description = descriptionValue,
                generateDescription = generateDescription,
            };
            
            string requestUri = _uri + _createNewFoodUri;

            var json = JsonConvert.SerializeObject(recipe);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PutAsync(requestUri, content);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            string ingredientsString = "";
            if (App.currentAddedIngredientIds.Count > 0)
            {
                for (int i = 0; i < App.currentAddedIngredientIds.Count; i++)
                {
                    ingredientsString = App.currentAddedIngredientNames[i] + " " + App.currentAddedIngredientGrams[i] + "g\n";

                }
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = (JObject)JsonConvert.DeserializeObject(returnedData);
            //var localRecipe = new RecipeModel
            //{
            //    method = method,
            //    oilId = oilId,
            //    oilPortionMl = oilPortionMl,
            //    timeInMinutes = timeInMinutes,
            //    name = name,
            //    description = result.SelectToken("description").ToString(),
            //    fat = fat,
            //    protein = protein,
            //    kcal = kcal,
            //    carbohydrate = carbohydrate,
            //    ingredientsString = ingredientsString
            //};
            //App.saveLocalRecipe = localRecipe;
            return true;
        }

        public static async Task<bool> GetAllRecipeByAuthId(int id)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _getAllRecipeByAuthIdUri + id.ToString();

            var response = await _client.GetAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<List<ComplexRecipeModel>> GetAllRecipeByAuthIdAsList(int id)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _getAllRecipeByAuthIdUri + id.ToString();

            var response = await _client.GetAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<ComplexRecipeModel>>(returnedData);
            return result;
        }

        public static async Task<List<ComplexRecipeModel>> GetRecipeByName(string name)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _getRecipeByNameUri + name;

            var response = await _client.GetAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<ComplexRecipeModel>>(returnedData);
            return result;
        }

        public static async Task<bool> DeleteRecipe(int id)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _deleteFoodUri + id.ToString();

            var response = await _client.DeleteAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> RecipeLikeUnlike(int id)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _likeUnlikeRecipeUri + id.ToString();

            var json = JsonConvert.SerializeObject(id);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PatchAsync(requestUri, content);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        // ------------------------------------ Recipe END ------------------------------------------------------------------------
        // ------------------------------------ Last24h START ------------------------------------------------------------------------
        public static async Task<List<ComplexLast24hModel>> GetLast24h(DateTime? date = null)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = "";
            if (date != null)
            {
                requestUri = _uri + _getLast24hByDateUri + date;
            }
            else
            {
                requestUri = _uri + _getAllLast24hUri + date;
            }

            var response = await _client.GetAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<ComplexLast24hModel>>(returnedData);
            //var result = (JArray)JsonConvert.DeserializeObject(returnedData);
            return result;
        }

        public static async Task<bool> PostLast24h(NewMeal meal)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _postLast24hUri;

            var json = JsonConvert.SerializeObject(meal);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PutAsync(requestUri, content);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var returnedData = await response.Content.ReadAsStringAsync();
            //var result = (JArray)JsonConvert.DeserializeObject(returnedData);
            return true;
        }

        public static async Task<bool> PortionModifier(int id, string portion)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri;
            switch (portion)
            {
                case "Half portion":
                    requestUri += _halfPortionUri;
                    break;
                case "Third portion":
                    requestUri += _thirdPortionUri;
                    break;
                case "Quarter portion":
                    requestUri += _quarterPortionUri;
                    break;
                case "Double portion":
                    requestUri += _doublePortionUri;
                    break;
                default:
                    break;
            }
            requestUri += id.ToString();
            var json = JsonConvert.SerializeObject(id);
            var content = new StringContent(json, Encoding.UTF8, _contentType);
            var response = await _client.PatchAsync(requestUri, content);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public static async Task<bool> DeleteLast24hById(int id)
        {
             
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", App.jwtToken);
            string requestUri = _uri + _deleteLast24ByIdUri + id.ToString();
            var response = await _client.DeleteAsync(requestUri);
             
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        // ------------------------------------ Last24h END ------------------------------------------------------------------------
    }
}
