using System.Collections.ObjectModel;
using FirstLab.controls.homePage;
using FirstLab.viewModels.home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<MeasurementVmItem> MeasurementItems;

        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            HomePageStackLayout.Children.Add(CreateStatusIndicator());
            HomePageStackLayout.Children.Add(CreateErrorLabel());

            var listView = MeasurementsList.CreateMeasurementsListView(MeasurementItems, viewModel.MyCommand);
            HomePageStackLayout.Children.Add(listView);
        }

        private StackLayout HomePageStackLayout => homePageStackLayout;

        private static ActivityIndicator CreateStatusIndicator()
        {
            var indicator = new ActivityIndicator
            {
                IsRunning = false, IsVisible = false
            };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding(nameof(HomeViewModel.IsLoading)));
            indicator.SetBinding(IsVisibleProperty, new Binding(nameof(HomeViewModel.IsLoading)));
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