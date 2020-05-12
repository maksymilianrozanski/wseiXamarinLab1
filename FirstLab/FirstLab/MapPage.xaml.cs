using System.Collections.ObjectModel;
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

            BindingContext = viewModel;
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
                    return p;
                }),
            };
            map.SetBinding(Map.ItemsSourceProperty, new Binding(nameof(HomeViewModel.MapLocations)));
            Content = map;
        }
    }
}