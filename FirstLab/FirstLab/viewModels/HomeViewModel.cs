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
        public HomeViewModel(INavigation navigation) : base(navigation)
        {
            this.MyCommand = new Command<MeasurementVmItem>(
                execute: vmListItem => { navigation.PushAsync(new DetailsPage(vmListItem)); }
            );
            
            _measurementVmItems = MeasurementsInstallationToVmItem(new List<(Measurements, Installation)>
                {(_measurementStub, _installationStub)});
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

        private readonly Installation _installationStub = new Installation(8077, new Location(50.062006, 19.940984),
            new Address("Poland", "Krak├│w", "Miko┼éajska"));

        public ICommand MyCommand { get; set; }

        private List<MeasurementVmItem> _measurementVmItems;

        public List<MeasurementVmItem> MeasurementInstallationVmItems
        {
            get => _measurementVmItems;
            set => SetProperty(ref _measurementVmItems, value);
        }

        public static List<MeasurementVmItem> MeasurementsInstallationToVmItem(
            IEnumerable<(Measurements, Installation)> items) =>
            items.Select(it => (it.Item1.current.values, it.Item2))
                .SelectMany(valuesInst => valuesInst.values, (tuple, value) => (value, tuple.Item2))
                .Select(it => new MeasurementVmItem
                {
                    Country = it.Item2.address.country,
                    City = it.Item2.address.city,
                    Street = it.Item2.address.street,
                    Name = it.value.name,
                    Value = it.value.value
                }).ToList();
    }

    public struct MeasurementVmItem
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }
}