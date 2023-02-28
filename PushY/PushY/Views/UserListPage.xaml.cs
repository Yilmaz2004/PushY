using PushY.Models;
using PushY.ViewModels;
using PushY.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PushY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserListPage : ContentPage
    {
        public UserListPageVM viewModel;
        public UserListPage()
        {
            InitializeComponent();
            viewModel = new UserListPageVM();
            BindingContext = viewModel;
            viewModel.GetData();
        }

        public async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = e.SelectedItem as UserModel;

            UserChatModel chatModel = new UserChatModel
            {
                From_Id = await SecureStorage.GetAsync("UserId"),
                To_Id = selected.Id
            };

            App.Current.MainPage = new ChatPage(chatModel);
        }
    }
}