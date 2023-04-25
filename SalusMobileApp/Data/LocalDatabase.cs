using SQLite;
using SalusMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SalusMobileApp.Data
{
    public class LocalDatabase
    {
        readonly SQLiteAsyncConnection _connection;

        public LocalDatabase(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath);
            _connection.CreateTableAsync<LoginModel>().Wait();
            _connection.CreateTableAsync<UserProfileModel>().Wait();
            _connection.CreateTableAsync<EncryptionModel>().Wait();
        }

        public void DeleteUserTable()
        {
            _connection.DropTableAsync<UserProfileModel>().Wait();
        }

        public void SaveLoginData(LoginModel login)
        {
            _connection.InsertAsync(login);
        }

        public LoginModel GetLoginData()
        {
            var login = _connection.Table<LoginModel>().ToListAsync().Result;
            if (login.Count != 0)
            {
                return login[0];
            }
            return null;
        }

        public void DeleteLoginData()
        {
            _connection.DeleteAllAsync<LoginModel>();
        }

        public void OfflineLogin()
        {
            
        }

        public void SaveEncryptionData(EncryptionModel data)
        {
            _connection.InsertAsync(data);
        }

        public EncryptionModel GetEncryptionData()
        {
            var data = _connection.Table<EncryptionModel>().ToListAsync().Result;
            if (data.Count != 0)
            {
                return data[0];
            }
            return null;
        }

        public void DeleteEncryptionData()
        {
            _connection.DeleteAllAsync<EncryptionModel>();
        }

        public void SaveLocalUserProfileData(UserProfileModel profileModel)
        {
            if(App.database.GetLocalUserProfileData() != null)
            {
                _connection.DeleteAllAsync<UserProfileModel>();
            }
            _connection.InsertAsync(profileModel);
            
        }

        public UserProfileModel GetLocalUserProfileData()
        {
            var profile = _connection.Table<UserProfileModel>().ToListAsync().Result;
            if(profile.Count != 0)
            {
                return profile[0];
            }
            return null;
        }

        public void DeleteLocalUserProfile()
        {
            _connection.DeleteAllAsync<UserProfileModel>();
        }

        public ObservableCollection<UserProfileModel> GetLocalUserProfileDataForViewModel()
        {
            return new ObservableCollection<UserProfileModel>(_connection.Table<UserProfileModel>().ToListAsync().Result);
        }

        public ObservableCollection<RecipeModel> GetRecipe()
        {
            throw new Exception();
        }

        public void DeleteAll()
        {
            App.database.DeleteLoginData();
            App.database.DeleteEncryptionData();
            App.database.DeleteUserTable();
        }
    }
}
