using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private StackLayout HomePageStackLayout => homePageStackLayout;

        public Button HomePageButton;

        public HomePage()
        {
            InitializeComponent();
            HomePageButton = new Button
            {
                Text = "This is home page button",
            };
            HomePageButton.Clicked += (sender, e) => Navigation.PushAsync(new DetailsPage());

            HomePageStackLayout.Children.Add(HomePageButton);
        }
    }
}