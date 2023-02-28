using Newtonsoft.Json;
using PushY.Models;
using PushY.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PushY.Services
{
    public class Services
    {
        public static async Task<string> Register(UserModel model)
        {
            HandleApiCall apiCall = new HandleApiCall();
            var json = JsonConvert.SerializeObject(model);
            var result = await apiCall.DoCall("Register", json);
            return result;
        }

        public static async Task<string> Login(UserModel model)
        {
            HandleApiCall apiCall = new HandleApiCall();
            var json = JsonConvert.SerializeObject(model);
            var result = await apiCall.DoCall("Login", json);
            return result;
        }

        public static async Task<List<UserModel>> AllUsers()
        {
            List<UserModel> users = new List<UserModel>();

            HandleApiCall apiCall = new HandleApiCall();

            var result = await apiCall.DoCall("AllUsers", await SecureStorage.GetAsync("UserId"));
            users = JsonConvert.DeserializeObject<List<UserModel>>(result);

            return users;
        }

        public static async Task<List<UserChatModel>> UserChat(UserChatModel model)
        {
            List<UserChatModel> users = new List<UserChatModel>();

            HandleApiCall apiCall = new HandleApiCall();

            var json = JsonConvert.SerializeObject(model);
            var result = await apiCall.DoCall("UserChat", json);
            if (result == null || result == "NOK")
            {
                await App.Current.MainPage.DisplayAlert("Failed", "Something went wrong. try again later", "OK");
            }
            else if (result == "NMSG")
            {
                await App.Current.MainPage.DisplayAlert("Message", "No messages found", "OK");
            }
            else
            {
                try
                {
                    users = JsonConvert.DeserializeObject<List<UserChatModel>>(result);
                }
                catch (Exception)
                {
                    await App.Current.MainPage.DisplayAlert("Failed", "Something went wrong. try again later", "OK");
                }
            }

            return users;
        }

        public async static Task<string> TokenRefresh(string token)
        {
            UserModel model = new UserModel()
            {
                Token = token,
                Id = await SecureStorage.GetAsync("id")
            };

            HandleApiCall apiCall = new HandleApiCall();

            var json = JsonConvert.SerializeObject(model);
            var result = await apiCall.DoCall("TokenRefresh", "");

            return result;
        }

        public async static Task<string> SendMessage(UserChatModel model)
        {

            HandleApiCall apiCall = new HandleApiCall();

            var json = JsonConvert.SerializeObject(model);
            var result = await apiCall.DoCall("SendMessage", json);

            return result;
        }
    }
}
