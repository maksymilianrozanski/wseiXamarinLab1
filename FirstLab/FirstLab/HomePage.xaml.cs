using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FirstLab.controls.homePage;
using FirstLab.location;
using FirstLab.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;

namespace FirstLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private StackLayout HomePageStackLayout => homePageStackLayout;

        public Button HomePageButton;

        public ObservableCollection<MeasurementVmItem> MeasurementItems;

        public HomePage()
        {
            InitializeComponent();

            var vm = new HomeViewModel(Navigation);
            BindingContext = vm;

            HomePageButton = new Button {Text = "This is home page button", Command = vm.MyCommand,};

            HomePageStackLayout.Children.Add(HomePageButton);

            var listView = new ListView();
            listView.ItemsSource = MeasurementItems;
            listView.HasUnevenRows = true;
            listView.ItemTemplate = new DataTemplate(typeof(MeasurementCell));
            listView.SetBinding(ListView.ItemsSourceProperty,
                new Binding(nameof(HomeViewModel.MeasurementInstallationVmItems)));
            HomePageStackLayout.Children.Add(listView);

            Console.WriteLine("HomePage will print location");
            ReadLocation();
        }

        private static async Task ReadLocation()
        {
            var location = await LocationProvider.GetLocation();
            Console.WriteLine("Printing location from HomePage: " + location);
        }
    }
}