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
    public class CommentsViewModel
    {
        public ObservableCollection<CommentModel> Comments { get; set; }
        public event EventHandler CommentsLoaded;
        public CommentsViewModel()
        {
            Comments = new ObservableCollection<CommentModel>();
        }

        public async void GetCommentsById(int id)
        {
            if (ServiceValidation.InternetConnectionValidator())
            {
                var data = await RestServices.GetCommentsByEmail(id);
                if (data != null)
                {
                    Comments = new ObservableCollection<CommentModel>(data);
                    if (CommentsLoaded != null)
                    {
                        CommentsLoaded(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
