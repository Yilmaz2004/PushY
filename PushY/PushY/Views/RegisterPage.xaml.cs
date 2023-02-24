using PushY.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PushY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        RegisterPageVM viewModel;
        public RegisterPage()
        {
            InitializeComponent();
            viewModel = new RegisterPageVM();
            BindingContext = viewModel;
        }
    }
}