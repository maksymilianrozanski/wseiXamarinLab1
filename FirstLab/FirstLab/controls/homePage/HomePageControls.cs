using System.Collections.ObjectModel;
using System.Windows.Input;
using FirstLab.viewModels;
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
            };
            listView.ItemTapped += (sender, args) => vm.Execute(args.Item);
            listView.SetBinding(ListView.ItemsSourceProperty,
                new Binding(nameof(HomeViewModel.MeasurementInstallationVmItems)));
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
            var nameLabel = new Label();
            nameLabel.SetBinding(Label.TextProperty, nameof(MeasurementVmItem.Name));
            var valueLabel = new Label();
            valueLabel.SetBinding(Label.TextProperty, nameof(MeasurementVmItem.Value));

            stackLayout.Children.Add(countryLabel);
            stackLayout.Children.Add(cityLabel);
            stackLayout.Children.Add(streetLabel);
            stackLayout.Children.Add(nameLabel);
            stackLayout.Children.Add(valueLabel);

            View = stackLayout;
        }
    }
}