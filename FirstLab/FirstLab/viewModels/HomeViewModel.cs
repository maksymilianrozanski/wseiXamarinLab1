using System;
using System.Collections.Generic;
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
            Items = new List<HomeViewModelItem>
            {
            };

            var thread = new Thread(() =>
            {
                Thread.Sleep(1000);
                while (true)
                {
                    var now = DateTime.Now.Second;
                    Items = new List<HomeViewModelItem>
                    {
                        new HomeViewModelItem {Name = "someText" + now},
                        new HomeViewModelItem {Name = "someText" + now + 2}
                    };
                }
            });
            thread.Start();
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

        public const string MeasurementsBindName = nameof(Measurements);
        public const string HomeViewModelItemBindName = nameof(Items);
        private List<(Measurements, int)> _measurements;

        public ICommand MyCommand { get; set; }

        private List<HomeViewModelItem> _items;

        public List<(Measurements, int)> Measurements
        {
            get => _measurements;
            set => SetProperty(ref _measurements, value);
        }

        public List<HomeViewModelItem> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
    }

    public class HomeViewModelItem
    {
        public string Name { get; set; }
    }

    public class HomeItemTemplate : ViewCell
    {
        public HomeItemTemplate()
        {
            var typeLabel = new Label();
            typeLabel.SetBinding(Label.TextProperty, new Binding("Name"));
            View = typeLabel;
        }
    }
}