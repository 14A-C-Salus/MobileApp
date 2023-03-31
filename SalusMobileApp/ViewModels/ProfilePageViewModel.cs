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
        public ProfilePageViewModel()
        {
            UserProfile = new ObservableCollection<UserProfileModel>();
        }

        public void GetUserProfileFromViewModel()
        {
            UserProfile = App.database.GetLocalUserProfileDataForViewModel();
        }
    }
}
