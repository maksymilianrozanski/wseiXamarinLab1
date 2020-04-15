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

            HomePageStackLayout.Children.Add(CreateStatusIndicator());
            HomePageStackLayout.Children.Add(CreateErrorLabel());

            var listView = MeasurementsList.CreateMeasurementsListView(MeasurementItems, vm.MyCommand);
            HomePageStackLayout.Children.Add(listView);
        }

        private StackLayout HomePageStackLayout => homePageStackLayout;

        private static ActivityIndicator CreateStatusIndicator()
        {
            var indicator = new ActivityIndicator
            {
                IsRunning = false
            };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding(nameof(HomeViewModel.IsLoading)));
            return indicator;
        }

        private static Label CreateErrorLabel()
        {
            var label = new Label
            {
                TextColor = Color.Red,
                FontSize = 20.0
            };
            label.SetBinding(Label.TextProperty, new Binding(nameof(HomeViewModel.ErrorMessage)));
            return label;
        }
    }
}