using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FirstLab.db;
using FirstLab.entities;
using FirstLab.location;
using FirstLab.network;
using FirstLab.network.models;
using LaYumba.Functional;
using Xamarin.Essentials;
using Xamarin.Forms;
using MeasurementById =
    System.Func<int, LaYumba.Functional.Either<LaYumba.Functional.Error, FirstLab.network.models.Measurements>>;
using FetchInstallationsByLocation =
    System.Func<Xamarin.Essentials.Location, LaYumba.Functional.Either<LaYumba.Functional.Error,
        System.Collections.Generic.List<FirstLab.network.models.Installation>>>;
using ReplaceInstallationsInDb =
    System.Func<System.Collections.Generic.List<FirstLab.network.models.Installation>, LaYumba.Functional.Either<
        LaYumba.Functional.Error, System.Collections.Generic.List<FirstLab.network.models.Installation>>>;
using ReplaceMeasurementInDb =
    System.Func<(FirstLab.network.models.Measurements, FirstLab.network.models.Installation), LaYumba.Functional.Either<
        LaYumba.Functional.Error, (FirstLab.network.models.Measurements, FirstLab.network.models.Installation)>>;
using FetchMeasurementsOfInstallation =
    System.Func<FirstLab.network.models.Installation, LaYumba.Functional.Either<LaYumba.Functional.Error, (
        FirstLab.network.models.Measurements, FirstLab.network.models.Installation)>>;

[assembly: InternalsVisibleTo("FirstLabUnitTests")]

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

            var fetchMeasurementsFromDbOrNetwork =
                FetchMeasurementsFromDbOrNetwork(() => DateTime.Now)(DatabaseHelper.LoadMeasurementByInstallationId2)
                    (_network.GetMeasurementsRequest);
            var installationByLocation = FetchInstallations;
            var replaceInstallationsInDb = DatabaseHelper.ReplaceInstallations(App.Database.Connection);

            FetchVmItems(fetchMeasurementsFromDbOrNetwork)(installationByLocation)(replaceInstallationsInDb)
                (DatabaseHelper.ReplaceCurrent2)(location)
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

        internal static Func<FetchMeasurementsOfInstallation,
            Func<FetchInstallationsByLocation,
                Func<ReplaceInstallationsInDb,
                    Func<ReplaceMeasurementInDb,
                        Func<Location,
                            Either<Error, (List<Error>, List<MeasurementVmItem>)>>>>>> FetchVmItems =>
            measurementsOfInstallation => installationByLocation =>
                replaceInstallations => replaceMeasurement =>
                    currentLocation =>
                        installationByLocation(currentLocation)
                            .Bind(replaceInstallations)
                            .Map(it => it.Map(measurementsOfInstallation))
                            .Map(it => it.Map(it2 => it2.Bind(replaceMeasurement)))
                            .Map(AggregateEithers)
                            .Match(error => (new List<Error>
                            {
                                error
                            }, new List<MeasurementVmItem>()), tuple =>
                                (tuple.Item1, MeasurementsInstallationToVmItem(tuple.Item2)));

        private static Func<MeasurementById, Func<Installation, Either<Error, (Measurements, Installation)>>>
            FetchMeasurements => networkGet => installation =>
            networkGet(installation.id).Map(measurement => (measurement, installation));

        internal static Func<
                Func<DateTime>,
                Func<Func<int, Either<Error, Option<CurrentEntity>>>,
                    Func<MeasurementById,
                        Func<Installation, Either<Error, (Measurements, Installation)>>>>>
            FetchMeasurementsFromDbOrNetwork
            => dateFunc => measurementFromDbByInstallationId => networkGet => installation =>
            {
                var eitherOption = measurementFromDbByInstallationId(installation.id);
                return eitherOption.Bind(it =>
                    it.Match(() => FetchMeasurements(networkGet)(installation),
                        entity => IsMeasurementObsolete(dateFunc, entity.ToMeasurement())
                            ? FetchMeasurements(networkGet)(installation)
                            : (entity.ToMeasurement(), installation)));
            };

        private FetchInstallationsByLocation FetchInstallations =>
            location => _network.GetNearestInstallationsRequest2(2)(location);

        internal static bool IsMeasurementObsolete(Func<DateTime> getTime, Measurements measurement)
        {
            if (!DateTime.TryParse(measurement.current.tillDateTime, out var tillDate)) return false;
            var difference = getTime().Subtract(tillDate).TotalMinutes;
            return difference >= 60;
        }

        /// <summary>
        /// returns true if none of installations is in maxDistance range (in kilometers)
        /// </summary>
        public static Func<double, Func<List<Installation>, Location, bool>> IsLocationChanged =>
            maxDistance => (installations, currentLocation) =>
                !installations.Exists(installation =>
                    installation.location.CalculateDistance(currentLocation, DistanceUnits.Kilometers) <= maxDistance);

        internal static (List<Error>, List<TR> ) AggregateEithers<TR>(IEnumerable<Either<Error, TR>> list) =>
            list.Aggregate((new List<Error>(), new List<TR>()), (acc, either) =>
            {
                either.Match(error => acc.Item1.Add(error),
                    value => acc.Item2.Add(value));
                return acc;
            });

        internal static List<MeasurementVmItem> MeasurementsInstallationToVmItem(
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