using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using FirstLab.network.models;
using Xamarin.Forms;

namespace FirstLab.viewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(INavigation navigation) : base(navigation)
        {
            this.MyCommand = new Command(
                execute: () => { navigation.PushAsync(new DetailsPage()); }
            );
            Measurements = new List<(Measurements, int)> {(_measurementStub, 8077)};
        }

        private readonly Measurements _measurementStub = new Measurements(new Current(
            "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
            new List<Value> {new Value("PM1", 13.61), new Value("PM25", 19.76)},
            new List<Index>
            {
                new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                    "Don't miss this day! The clean air calls!", "#D1CF1E")
            },
            new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}));

        private List<(Measurements, int)> _measurements;

        public ICommand MyCommand { get; set; }

        public List<(Measurements, int)> Measurements
        {
            get => _measurements;
            set => SetProperty(ref _measurements, value);
        }

        public List<MeasurementViewModelItem> MeasurementViewModelItems
        {
            get => _measurements.Select(it =>
                    (it.Item1.current, it.Item2)).Select(it => (it.Item1.values, it.Item2))
                .SelectMany(tuple => tuple.values, (tuple, i) => (tuple, i))
                .Select(it => new MeasurementViewModelItem {Name = it.i.name, Value = it.i.value}).ToList();
        }
    }

    public class MeasurementViewModelItem
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }

    public class MeasurementItemCellTemplate : ViewCell
    {
        public MeasurementItemCellTemplate()
        {
            var stackLayout = new StackLayout();
            var addressLabel = new Label();
            addressLabel.SetBinding(Label.TextProperty, nameof(MeasurementViewModelItem.Name));
            var valueLabel = new Label();
            valueLabel.SetBinding(Label.TextProperty, nameof(MeasurementViewModelItem.Value));
            stackLayout.Children.Add(addressLabel);
            stackLayout.Children.Add(valueLabel);
            View = stackLayout;
        }
    }
}