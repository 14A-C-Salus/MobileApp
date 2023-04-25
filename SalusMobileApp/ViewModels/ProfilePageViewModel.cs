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
    public class ProfilePageViewModel
    {
        public ObservableCollection<UserProfileModel> UserProfile { get; set; }
        public event EventHandler UserProfileLoaded;
        public ProfilePageViewModel()
        {
            UserProfile = new ObservableCollection<UserProfileModel>();
        }

        public async void GetUserProfileFromViewModel()
        {
            if(ServiceValidation.InternetConnectionValidator())
            {
                var data = await RestServices.GetUserProfileDataAsObject(int.Parse(App.userId));
                if(data != null)
                {
                    UserProfile = new ObservableCollection<UserProfileModel>
                    {
                        data
                    };
                    if (UserProfileLoaded != null)
                    {
                        UserProfileLoaded(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                UserProfile = App.database.GetLocalUserProfileDataForViewModel();
                if(UserProfileLoaded != null) 
                { 
                    UserProfileLoaded(this, EventArgs.Empty);
                }
            }
            
        }
    }
}
