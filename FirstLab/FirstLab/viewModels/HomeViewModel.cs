using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using FirstLab.location;
using FirstLab.network;
using FirstLab.network.models;
using Xamarin.Forms;

namespace FirstLab.viewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private List<MeasurementVmItem> _measurementVmItems;

        public HomeViewModel(INavigation navigation) : base(navigation)
        {
            MyCommand = new Command<MeasurementVmItem>(
                vmListItem => { navigation.PushAsync(new DetailsPage(vmListItem)); }
            );

            LoadValues();
        }

        public ICommand MyCommand { get; set; }

        public List<MeasurementVmItem> MeasurementInstallationVmItems
        {
            get => _measurementVmItems;
            set => SetProperty(ref _measurementVmItems, value);
        }

        private async void LoadValues()
        {
            var location = await LocationProvider.GetLocation();
            var httpClient = new HttpClient {BaseAddress = new Uri("https://airapi.airly.eu")};
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("apiKey", App.ApiKey);

            var network = new Network(httpClient);
            var nearestInstallation = network.GetNearestInstallationsRequest(location).Result;
            var installation = Network.GetNearestInstallation(nearestInstallation);
            var id = installation.id;

            var measurementsResponse = await network.GetMeasurementsRequest(id);
            var measurements = Network.GetMeasurements(measurementsResponse);
            MeasurementInstallationVmItems = MeasurementsInstallationToVmItem(new List<(Measurements, Installation)>
            {
                (measurements, installation)
            });
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