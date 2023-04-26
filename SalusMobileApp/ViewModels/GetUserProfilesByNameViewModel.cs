using Newtonsoft.Json.Linq;
using SalusMobileApp.Data;
using SalusMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalusMobileApp.ViewModels
{
    public class GetUserProfilesByNameViewModel
    {
        public ObservableCollection<UserDataModel> UserProfiles { get; set; }
        public event EventHandler UserProfilesLoaded;
        public GetUserProfilesByNameViewModel()
        {
            UserProfiles = new ObservableCollection<UserDataModel>();
        }

        public async Task<bool> GetUserProfilesByName(string name)
        {
            if (ServiceValidation.InternetConnectionValidator())
            {
                var data = await RestServices.GetUserProfileByName(name);
                if (data != null)
                {
                    UserProfiles = new ObservableCollection<UserDataModel>(data);
                    UserProfilesLoaded?.Invoke(this, EventArgs.Empty);
                    return true;
                }
            }
            return false;
        }
    }
}
