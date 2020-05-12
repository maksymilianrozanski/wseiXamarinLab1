using System.Collections.ObjectModel;
using System.Linq;
using FirstLab.network.models;
using FirstLab.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FirstLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public ObservableCollection<MapLocation> MapLocations;

        public MapPage(HomeViewModel viewModel)
        {
            InitializeComponent();
            var map = new Map
            {
                IsShowingUser = true,
                ItemsSource = MapLocations,
                ItemTemplate = new DataTemplate(() =>
                {
                    var p = new Pin();
                    p.SetBinding(Pin.PositionProperty, nameof(MapLocation.Position));
                    p.SetBinding(Pin.AddressProperty, nameof(MapLocation.Address));
                    p.SetBinding(Pin.LabelProperty, nameof(MapLocation.Description));
                    p.InfoWindowClicked += (sender, args) =>
                    {
                        var item = viewModel.MeasurementInstallationVmItems.First(it =>
                            it.Installation.address.ToString().Equals((sender as Pin).Address.ToString()));
                        viewModel.MyCommand.Execute(item);
                    };
                    return p;
                }),
            };
            map.SetBinding(Map.ItemsSourceProperty, new Binding(nameof(HomeViewModel.MapLocations)));
            BindingContext = viewModel;
            Content = map;
        }
    }
}