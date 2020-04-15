using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using FirstLab.location;
using FirstLab.network;
using FirstLab.network.models;
using LaYumba.Functional;
using Xamarin.Forms;

namespace FirstLab.viewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private string _errorMessage;
        private bool _isLoading;
        private List<MeasurementVmItem> _measurementVmItems;

        public HomeViewModel(INavigation navigation) : base(navigation)
        {
            MyCommand = new Command<MeasurementVmItem>(
                vmListItem => { navigation.PushAsync(new DetailsPage(vmListItem)); }
            );

            LoadValues();
        }

        public ICommand MyCommand { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<MeasurementVmItem> MeasurementInstallationVmItems
        {
            get => _measurementVmItems;
            set => SetProperty(ref _measurementVmItems, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private async void LoadValues()
        {
            IsLoading = true;
            var location = await LocationProvider.GetLocation();
            var httpClient = new HttpClient {BaseAddress = new Uri("https://airapi.airly.eu")};
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("apiKey", App.ApiKey);

            var network = new Network(httpClient);

            network.GetNearestInstallationsRequest(location)
                .Bind(it => MeasurementInstallationPair(network.GetMeasurementsRequest2(it.id), it))
                .Bind<Error, (Measurements, Installation), List<(Measurements, Installation)>>(it =>
                    new List<(Measurements, Installation)> {it})
                .Bind<Error, List<(Measurements, Installation)>, List<MeasurementVmItem>>(it =>
                    MeasurementsInstallationToVmItem(it))
                .Match(error =>
                    {
                        ErrorMessage = "Something went wrong...";
                        Console.WriteLine(error.Message);
                    },
                    list =>
                    {
                        MeasurementInstallationVmItems = list;
                        ErrorMessage = "";
                    });
            IsLoading = false;
        }

        private static Either<Error, (Measurements, Installation)> MeasurementInstallationPair(
            Either<Error, Measurements> m,
            Installation i)
        {
            return m.Bind<Error, Measurements, (Measurements, Installation)>(measurement
                => (measurement, i));
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