using System.Windows.Input;
using Xamarin.Forms;

namespace FirstLab.viewModels
{
    public class HomeViewModel
    {
        public HomeViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.MyCommand = new Command(
                execute: () => { navigation.PushAsync(new DetailsPage()); }
            );
        }

        public INavigation Navigation { get; set; }

        public ICommand MyCommand { get; set; }
    }
}