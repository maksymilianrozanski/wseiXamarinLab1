using System;
using FirstLab.styles;
using FirstLab.viewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace FirstLab
{
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            StackLayout.Children.Add(new Image
            {
                Source = ImageSource.FromUri(new Uri("https://cdn.airly.eu/assets/brand/logo/primary/airly-1024.png"))
            });

            StackLayout.Children.Add(new Label
            {
                Text = "This app displays data from airly api\nPress Home at the bottom to start",
                TextColor = Colors.TextColorMain,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            });

            var homeViewModel = new HomeViewModel(Navigation);

            Children.Add(new NavigationPage(new HomePage(homeViewModel))
            {
                Title = "Home",
                IconImageSource =
                    ImageSource.FromUri(new Uri("https://cdn.airly.eu/assets/brand/logo/primary/airly-1024.png"))
            });

            Children.Add(new NavigationPage(new MapPage(homeViewModel))
            {
                Title = "Map",
                IconImageSource = "map_icon.png"
            });
            Children.Add(new NavigationPage(new SettingsPage()) {Title = "Settings"});
        }

        private StackLayout StackLayout => TabbedPageStackLayout;
    }
}