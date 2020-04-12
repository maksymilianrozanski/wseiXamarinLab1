using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FirstLab.network.models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FirstLab.viewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly Installation _installationStub = new Installation(8077, new Location(50.062006, 19.940984),
            new Address("Poland", "Krak├│w", "Miko┼éajska"));

        private readonly Measurements _measurementStub = new Measurements(new Current(
            "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
            new List<Value>
            {
                new Value("PM10", 13.61), new Value("PM25", 19.76),
                new Value("PRESSURE", 1031.43), new Value("HUMIDITY", 33.51),
                new Value("TEMPERATURE", 14.07)
            },
            new List<Index>
            {
                new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                    "Don't miss this day! The clean air calls!", "#D1CF1E")
            },
            new List<Standard>
            {
                new Standard("WHO", "PM25", 25.0, 79.05),
                new Standard("WHO", "PM10", 50.0, 75.03)
            }));

        private List<MeasurementVmItem> _measurementVmItems;

        public HomeViewModel(INavigation navigation) : base(navigation)
        {
            MyCommand = new Command<MeasurementVmItem>(
                vmListItem => { navigation.PushAsync(new DetailsPage(vmListItem)); }
            );

            _measurementVmItems = MeasurementsInstallationToVmItem(new List<(Measurements, Installation)>
                {(_measurementStub, _installationStub), (_measurementStub, _installationStub)});
        }

        public ICommand MyCommand { get; set; }

        public List<MeasurementVmItem> MeasurementInstallationVmItems
        {
            get => _measurementVmItems;
            set => SetProperty(ref _measurementVmItems, value);
        }

        public static List<MeasurementVmItem> MeasurementsInstallationToVmItem(
            IEnumerable<(Measurements, Installation)> items)
        {
            return items.Select(it => (it.Item1, it.Item2))
                .Select(it => new MeasurementVmItem
                {
                    Measurements = it.Item1,
                    Installation = it.Item2,
                    City = it.Item2.address.city,
                    Country = it.Item2.address.country,
                    Street = it.Item2.address.street
                }).ToList();
        }
    }

    public struct MeasurementVmItem
    {
        public Measurements Measurements { get; set; }
        public Installation Installation { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}