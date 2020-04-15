using System.Collections.ObjectModel;
using FirstLab.controls.homePage;
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

            HomePageStackLayout.Children.Add(new ActivityIndicator
            {
                IsRunning = false
            });

            var listView = MeasurementsList.CreateMeasurementsListView(MeasurementItems, vm.MyCommand);
            HomePageStackLayout.Children.Add(listView);
        }

        private StackLayout HomePageStackLayout => homePageStackLayout;
    }
}