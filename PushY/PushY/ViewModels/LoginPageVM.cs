using PushY.Models;
using PushY.Views;
using System;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PushY.ViewModels
{
    public class LoginPageVM : ViewModelBase
    {
        bool block;
        private string _pwd;
        public string Pwd
        {
            get
            {
                return _pwd;
            }
            set
            {
                SetProperty(ref _pwd, value);
                LoginCmd.ChangeCanExecute();
            }
        }
        private string _nickname;
        public string NickName
        {
            get
            {
                return _nickname;
            }
            set
            {
                SetProperty(ref _nickname, value);
                LoginCmd.ChangeCanExecute();
            }
        }
        public Command LoginCmd { get; private set; }
        public Command RegisterCmd { get; private set; }
        public LoginPageVM()
        {
            LoginCmd = new Command(ExecuteLoginCmd, CanClick);
            block = false;

            RegisterCmd = new Command(ExecuteRegisterCmd);
        }
        public void ExecuteRegisterCmd()
        {
            App.Current.MainPage = new RegisterPage();
        }
        public async void ExecuteLoginCmd()
        {
            if (block)
                return;

            block = true;
            var md5 = GenerateMD5(_pwd);
            UserModel model = new UserModel
            {
                NickName = _nickname,
                Password = md5
            };
            var result = await Services.Services.Login(model);

            if (result == "NOK")
            {
                await App.Current.MainPage.DisplayAlert("Failed", "Nickname already Exists", "OK");
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

            Console.WriteLine(await SecureStorage.GetAsync("UserId"));
            block = false;
        }
        private bool CanClick()
        {
            return (!string.IsNullOrEmpty(_pwd) && !string.IsNullOrEmpty(_nickname));
        }
        public static string GenerateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
