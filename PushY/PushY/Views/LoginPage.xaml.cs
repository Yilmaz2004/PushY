using PushY.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PushY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginPageVM viewModel;
        public LoginPage()
        {
            InitializeComponent();
            viewModel = new LoginPageVM();
            BindingContext = viewModel;
        }
    }
}