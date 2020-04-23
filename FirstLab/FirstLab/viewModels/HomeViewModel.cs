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
using MeasurementById =
    System.Func<int, LaYumba.Functional.Either<LaYumba.Functional.Error, FirstLab.network.models.Measurements>>;
using InstallationsByLocation =
    System.Func<Xamarin.Essentials.Location, LaYumba.Functional.Either<LaYumba.Functional.Error,
        System.Collections.Generic.List<FirstLab.network.models.Installation>>>;

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

        private static Func<MeasurementById, Func<List<Installation>,
                List<Either<Error, (Measurements, Installation)>>>>
            FetchMeasurementsOfInstallations =>
            networkGet => installations => installations.Select(
                FetchMeasurements(networkGet)).ToList();

        private static Func<MeasurementById, Func<Installation, Either<Error, (Measurements, Installation)>>>
            FetchMeasurements => networkGet => installation =>
            networkGet(installation.id).Map(measurement => (measurement, installation));

        private static Func<MeasurementById, Func<InstallationsByLocation,
            Func<Location, Either<Error, (List<Error>, List<MeasurementVmItem>)>>>> FetchVmItems =>
            measurementById => installationByLocation => currentLocation =>
                installationByLocation(currentLocation)
                    .Map(FetchMeasurementsOfInstallations(measurementById))
                    .Map(AggregateEithers)
                    .Match(error => (new List<Error> {error}, new List<MeasurementVmItem>()), tuple =>
                        (tuple.Item1, MeasurementsInstallationToVmItem(tuple.Item2)));

        private InstallationsByLocation FetchInstallations =>
            location => _network.GetNearestInstallationsRequest2(2)(location);

        private async void LoadMultipleValues()
        {
            IsLoading = true;
            var location = await LocationProvider.GetLocation();

            FetchVmItems(_network.GetMeasurementsRequest)(FetchInstallations)(location)
                .Match(error =>
                {
                    ErrorMessage = "Something went wrong...";
                    Console.WriteLine(error.Message);
                }, list =>
                {
                    var (errors, measurementVmItems) = list;
                    MeasurementInstallationVmItems = measurementVmItems;
                    var numberOfErrors = errors.Count;
                    ErrorMessage = numberOfErrors > 0 ? "Number of errors: " + numberOfErrors : "";
                });
            IsLoading = false;
        }

        public static (List<Error>, List<TR> ) AggregateEithers<TR>(IEnumerable<Either<Error, TR>> list) =>
            list.Aggregate((new List<Error>(), new List<TR>()), (acc, either) =>
            {
                either.Match(error => acc.Item1.Add(error),
                    value => acc.Item2.Add(value));
                return acc;
            });

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