using FirstLab.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private StackLayout HomePageStackLayout => homePageStackLayout;

        public Button HomePageButton;

        public HomePage()
        {
            InitializeComponent();

            var vm = new HomeViewModel(Navigation);
            BindingContext = vm;

            HomePageButton = new Button {Text = "This is home page button", Command = vm.MyCommand,};

            HomePageStackLayout.Children.Add(HomePageButton);
        }
    }
}