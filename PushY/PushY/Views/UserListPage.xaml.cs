using PushY.Models;
using PushY.ViewModels;
using PushY.Views;
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

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = e.SelectedItem as UserModel;


        }
    }
}