using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FirstLab.location;
using FirstLab.network;
using FirstLab.network.models;
using LaYumba.Functional;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FirstLab.viewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly Network _network;
        private string _errorMessage;
        private bool _isLoading;
        private List<MeasurementVmItem> _measurementVmItems;

        public HomeViewModel(INavigation navigation) : base(navigation)
        {
            var httpClient = Network.CreateClient();
            _network = new Network(httpClient);

            MyCommand = new Command<MeasurementVmItem>(
                vmListItem => { navigation.PushAsync(new DetailsPage(vmListItem)); }
            );

            LoadMultipleValues();
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

        private async void LoadMultipleValues()
        {
            IsLoading = true;
            var location = await LocationProvider.GetLocation();

            FetchVmItems(location, _network)
                .Match(error =>
                {
                    ErrorMessage = "Something went wrong...";
                    Console.WriteLine(error.Message);
                }, list =>
                {
                    MeasurementInstallationVmItems = list;
                    ErrorMessage = "";
                });
            IsLoading = false;
        }

        private Either<Error, List<MeasurementVmItem>> FetchVmItems(Location location, Network network) =>
            network.GetNearestInstallationsRequest(location, 2)
                .Map(it => FetchMeasurementsOfInstallations(it, _network))
                .Bind(MeasurementsInstallationListToVmItems);

        private static List<Either<Error, (Measurements, Installation)>>
            FetchMeasurementsOfInstallations(List<Installation> it, Network network) =>
            it.Select(it2 => FetchMeasurements(it2, network)).ToList();

        private static Either<Error, (Measurements, Installation)> FetchMeasurements(
            Installation installation, Network network) =>
            network.GetMeasurementsRequest(installation.id)
                .Map(measurement => (measurement, installation));

        public static Either<Error, List<MeasurementVmItem>> MeasurementsInstallationListToVmItems(
            List<Either<Error, (Measurements, Installation)>> list) =>
            list.Select(MeasurementsInstallationToVmItem3).ToList();

        public static MeasurementVmItem MeasurementsInstallationToVmItem3(
            Either<Error, (Measurements, Installation)> measurementInstallation)
        {
            var (measurements, installation) = GetValueFromEither(measurementInstallation);
            return new MeasurementVmItem
            {
                Measurements = measurements,
                Installation = installation,
                City = installation.address.city,
                Country = installation.address.country,
                Street = installation.address.street
            };
        }

        private static T GetValueFromEither<T>(Either<Error, T> either)
        {
            var option = new Option<T>();
            either.Match(error => { }, arg => option = arg);
            return option.GetOrElse(() => null).Result;
        }

        public static List<MeasurementVmItem> MeasurementsInstallationToVmItem(
            IEnumerable<(Measurements, Installation)> items) =>
            items.Select(it => (it.Item1, it.Item2))
                .Select(it => new MeasurementVmItem
                {
                    Measurements = it.Item1,
                    Installation = it.Item2,
                    City = it.Item2.address.city,
                    Country = it.Item2.address.country,
                    Street = it.Item2.address.street
                }).ToList();
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