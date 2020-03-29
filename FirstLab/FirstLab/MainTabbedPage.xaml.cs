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
            Children.Add(new NavigationPage(new HomePage()) {Title = "Home"});
            Children.Add(new NavigationPage(new SettingsPage()) {Title = "Settings"});
        }
    }
}