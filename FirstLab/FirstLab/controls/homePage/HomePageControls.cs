using System.Collections.ObjectModel;
using System.Windows.Input;
using FirstLab.viewModels.home;
using Xamarin.Forms;

namespace FirstLab.controls.homePage
{
    public static class MeasurementsList
    {
        public static ListView CreateMeasurementsListView(ObservableCollection<MeasurementVmItem> listViewItemsSource,
            ICommand vm)
        {
            var listView = new ListView
            {
                ItemsSource = listViewItemsSource,
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(typeof(MeasurementCell)),
                IsPullToRefreshEnabled = true
            };
            listView.ItemTapped += (sender, args) => vm.Execute(args.Item);
            listView.SetBinding(ListView.ItemsSourceProperty,
                new Binding(nameof(HomeViewModel.MeasurementInstallationVmItems)));
            listView.SetBinding(ListView.RefreshCommandProperty,
                new Binding(nameof(HomeViewModel.ForceRefreshCommand)));
            listView.SetBinding(ListView.IsRefreshingProperty,
                new Binding(nameof(HomeViewModel.IsLoading)));
            return listView;
        }
    }

    public class MeasurementCell : ViewCell
    {
        public MeasurementCell()
        {
            var stackLayout = new StackLayout();
            var countryLabel = new Label();
            countryLabel.SetBinding(Label.TextProperty, nameof(MeasurementVmItem.Country));
            var cityLabel = new Label();
            cityLabel.SetBinding(Label.TextProperty, nameof(MeasurementVmItem.City));
            var streetLabel = new Label();
            streetLabel.SetBinding(Label.TextProperty, nameof(MeasurementVmItem.Street));

            stackLayout.Children.Add(countryLabel);
            stackLayout.Children.Add(cityLabel);
            stackLayout.Children.Add(streetLabel);

            View = stackLayout;
        }
    }
}