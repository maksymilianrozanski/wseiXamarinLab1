using System.Collections.Generic;
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

        public const string MeasurementsBindName = nameof(Measurements);
        private List<(Measurements, int)> _measurements;

        public ICommand MyCommand { get; set; }

        public List<(Measurements, int)> Measurements
        {
            get => _measurements;
            set => SetProperty(ref _measurements, value);
        }
    }
}