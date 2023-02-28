using Plugin.FirebasePushNotification;
using PushY.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PushY
{
    public partial class App : Application
    {
        public bool HasId;
        public static string UserId;

        public App()
        {
            InitializeComponent();
            CheckId();
        }

        async void CheckId()
        {
            var Id = await SecureStorage.GetAsync("UserId");

            if (string.IsNullOrEmpty(Id))
            {
                MainPage = new RegisterPage();
            }
            else
            {
                MainPage = new UserListPage();
                UserId = Id;
            }

        }
        protected override void OnStart()
        {
            CrossFirebasePushNotification.Current.OnTokenRefresh += async (s, p) =>
            {
                Console.WriteLine($"TOKEN REFRESH: {p.Token}");
                if (HasId)
                {
                    await Services.Services.TokenRefresh(p.Token);
                }
                else
                {
                    await SecureStorage.SetAsync("token", p.Token);
                }
            };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    var v = p.Data;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            };
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }



    }
}
