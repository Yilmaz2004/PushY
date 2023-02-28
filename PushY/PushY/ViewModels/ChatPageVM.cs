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
    public class ChatPageVM : ViewModelBase
    {
        bool block;

        UserChatModel myModel;
        public ObservableCollection<UserChatModel> UserChatList { get; set; }
        public Command BackCmd { get; private set; }
        public Command SendMessageCmd { get; private set; }

        public ChatPageVM()
        {
            SendMessageCmd = new Command(ExecuteSendMessageCmd, CanClick);
            block = false;
        }

        string _from_Id;
        public string From_Id
        {
            get
            {
                return _from_Id;
            }
            set
            {
                SetProperty(ref _from_Id, value);
                SendMessageCmd.ChangeCanExecute();
            }
        }

        string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                SetProperty(ref _message, value);
                SendMessageCmd.ChangeCanExecute();
            }
        }

        string _to_Id;
        public string To_Id
        {
            get
            {
                return _to_Id;
            }
            set
            {
                SetProperty(ref _to_Id, value);
                SendMessageCmd.ChangeCanExecute();
            }
        }

        private bool CanClick()
        {
            return (!string.IsNullOrEmpty(_message));
        }

        public ChatPageVM(UserChatModel chatModel)
        {
            UserChatList = new ObservableCollection<UserChatModel>();

            myModel = chatModel;

            BackCmd = new Command(ExecuteBackCmd);
            SendMessageCmd = new Command(ExecuteSendMessageCmd);
        }

        public void ExecuteBackCmd()
        {
            App.Current.MainPage = new UserListPage();
        }

        public async void ExecuteSendMessageCmd()
        {
            if (block)
                return;

            block = true;

            UserChatModel model = new UserChatModel
            {
                From_Id = _from_Id,
                Message = _message,
                To_Id = _to_Id,
            };
            var result = await Services.Services.SendMessage(model);

            if (result == "NOK")
            {
                await App.Current.MainPage.DisplayAlert("Failed", "asdg", "OK");
            }
            else
            {
                try
                {
                    await SecureStorage.SetAsync("UserId", result);
                    App.Current.MainPage = new UserListPage();
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "er is iets mis, pech", "OK");
                }
            }
        }

        public async void GetData()
        {
            UserChatList.Clear();

            var result = await Services.Services.UserChat(myModel);
            if (result != null)
            {
                foreach (var user in result)
                {
                    UserChatList.Add(user);
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Failed", "Something went wrong. try again later", "OK");
            }
        }


    }
}
