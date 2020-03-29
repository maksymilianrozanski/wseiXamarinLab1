using Xamarin.Forms;

namespace FirstLab
{
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();

            Children.Add(new NavigationPage(new HomePage()) {Title = "Home"});
            Children.Add(new NavigationPage(new SettingsPage()){Title = "Settings"});
        }
    }
}