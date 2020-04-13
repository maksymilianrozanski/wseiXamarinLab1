using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FirstLab.controls.homePage;
using FirstLab.location;
using FirstLab.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<MeasurementVmItem> MeasurementItems;

        public HomePage()
        {
            InitializeComponent();

            var vm = new HomeViewModel(Navigation);
            BindingContext = vm;

            var listView = MeasurementsList.CreateMeasurementsListView(MeasurementItems, vm.MyCommand);
            HomePageStackLayout.Children.Add(listView);

            Console.WriteLine("HomePage will print location");
            ReadLocation();
        }

        private StackLayout HomePageStackLayout => homePageStackLayout;

        private static async Task ReadLocation()
        {
            var location = await LocationProvider.GetLocation();
            Console.WriteLine("Printing location from HomePage: " + location);
        }
    }
}