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

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                SetProperty(ref _description, value);
                SendMessageCmd.ChangeCanExecute();
            }
        }

        private bool CanClick()
        {
            return (!string.IsNullOrEmpty(_description));
        }

        public ChatPageVM(UserChatModel chatModel)
        {
            UserChatList = new ObservableCollection<UserChatModel>();

            myModel = new UserChatModel();
            myModel = chatModel;

            BackCmd = new Command(ExecuteBackCmd);
            SendMessageCmd = new Command(ExecuteSendMessageCmd, CanClick);
            block = false;
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
                From_Id = App.UserId,
                Message = _description,
                To_Id = myModel.To_Id
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

                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "er is iets mis, pech", "OK");
                }
            }
            block = false;
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
