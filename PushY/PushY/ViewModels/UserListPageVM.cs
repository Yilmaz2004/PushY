using PushY.Models;
using PushY.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PushY.ViewModels
{
    public class UserListPageVM : ViewModelBase
    {
        public ObservableCollection<UserModel> UserList { get; set; }

        public Command LogoutCmd { get; private set; }
        public UserListPageVM()
        {
            UserList = new ObservableCollection<UserModel>();

            LogoutCmd = new Command(ExecuteLogoutCmd);
        }

        public void ExecuteLogoutCmd()
        {
            bool success = SecureStorage.Remove("UserId");
            App.Current.MainPage = new LoginPage();
        }


        public async void GetData()
        {
            UserList.Clear();

            var result = await Services.Services.AllUsers();
            if (result != null)
            {
                foreach (var user in result)
                {
                    UserList.Add(user);
                }
            }
        }
    }
}
