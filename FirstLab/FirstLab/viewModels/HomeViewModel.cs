using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using FirstLab.db;
using FirstLab.entities;
using FirstLab.location;
using FirstLab.network;
using FirstLab.network.models;
using LaYumba.Functional;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
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
using LoadInstallationsFromDb =
    System.Func<LaYumba.Functional.Either<LaYumba.Functional.Error,
        System.Collections.Generic.List<FirstLab.entities.InstallationEntity>>>;
using InstallationsReplacingFunc =
    System.Func<System.Collections.Generic.List<FirstLab.network.models.Installation>, LaYumba.Functional.Either<
        LaYumba.Functional.Error, System.Collections.Generic.List<FirstLab.network.models.Installation>>>;

[assembly: InternalsVisibleTo("FirstLabUnitTests")]

namespace FirstLab.viewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly Network _network;
        private string _errorMessage;
        private bool _isLoading;
        private List<MeasurementVmItem> _measurementVmItems;
        private List<MapLocation> _mapLocations;
        private const double MaxInstallationDistance = 100.0;

        public HomeViewModel(INavigation navigation) : base(navigation)
        {
            var httpClient = Network.CreateClient();
            _network = new Network(httpClient);

            MyCommand = new Command<MeasurementVmItem>(
                vmListItem => { navigation.PushAsync(new DetailsPage(vmListItem)); }
            );

            var normalLoading = NormalLoading(FetchVmItemsWithSavingFunctions);
            Task.Run(async () => await LoadMultipleValues(normalLoading));
        }

        public ICommand MyCommand { get; set; }

        public ICommand ForceRefreshCommand => new Command(async () =>
            await LoadMultipleValues(ForceLoading(FetchVmItemsWithSavingFunctions)));

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

        public List<MapLocation> MapLocations
        {
            get => _mapLocations;
            set => SetProperty(ref _mapLocations, value);
        }

        private static readonly Func<FetchMeasurementsOfInstallation, Func<FetchInstallationsByLocation,
            Func<Location, Either<Error, (List<Error>, List<MeasurementVmItem>)>>>> FetchVmItemsWithSavingFunctions
            = FetchVmItems(DatabaseHelper.ReplaceInstallations(App.Database.Connection))(DatabaseHelper
                .ReplaceCurrent2);

        private Func<Location, Either<Error, (List<Error>, List<MeasurementVmItem>)>> ForceLoading(
            Func<FetchMeasurementsOfInstallation, Func<FetchInstallationsByLocation,
                Func<Location, Either<Error, (List<Error>, List<MeasurementVmItem>)>>>> fetchWithSaving) =>
            fetchWithSaving(FetchMeasurements(_network.GetMeasurementsRequest))(FetchInstallations);

        private Func<Location, Either<Error, (List<Error>, List<MeasurementVmItem>)>> NormalLoading(
            Func<FetchMeasurementsOfInstallation, Func<FetchInstallationsByLocation,
                Func<Location, Either<Error, (List<Error>, List<MeasurementVmItem>)>>>> fetchWithSaving)
        {
            var measurementsFromDbOrNetwork =
                FetchMeasurementsFromDbOrNetwork(() => DateTime.Now)(DatabaseHelper.LoadMeasurementByInstallationId2)
                    (_network.GetMeasurementsRequest);

            var installationsFromDbOrNetwork = FetchInstallationsFromDbOrNetwork
                (DatabaseHelper.LoadInstallationEntities2)(DatabaseHelper.ReplaceInstallations2)(FetchInstallations);

            return fetchWithSaving(measurementsFromDbOrNetwork)(installationsFromDbOrNetwork);
        }

        private async Task LoadMultipleValues(
            Func<Location, Either<Error, (List<Error>, List<MeasurementVmItem>)>> fetchingFunc)
        {
            IsLoading = true;
            var location = await LocationProvider.GetLocation();
            await DisplayValues(fetchingFunc(location));
            IsLoading = false;
        }

        private async Task DisplayValues(Either<Error, (List<Error>, List<MeasurementVmItem>)> values)
        {
            values.Match(error =>
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
        }

        internal static List<MapLocation> CreateMapLocations(IEnumerable<MeasurementVmItem> vmItems) =>
            vmItems.Map(it => new MapLocation(it.City + " " + it.Street + " " + it.Country,
                "CAQI: " + it.Measurements.current.indexes.First().description,
                new Position(it.Installation.location.Latitude, it.Installation.location.Longitude))).ToList();

        internal static Func<ReplaceInstallationsInDb,
            Func<ReplaceMeasurementInDb,
                Func<FetchMeasurementsOfInstallation,
                    Func<FetchInstallationsByLocation,
                        Func<Location, Either<Error, (List<Error>, List<MeasurementVmItem>)>>>>>> FetchVmItems =>
            replaceInstallations => replaceMeasurement =>
                measurementsOfInstallation => installationByLocation =>
                    currentLocation =>
                        installationByLocation(currentLocation)
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

        public static Func<LoadInstallationsFromDb,
                Func<ReplaceInstallationsInDb,
                    Func<FetchInstallationsByLocation,
                        FetchInstallationsByLocation>>>
            FetchInstallationsFromDbOrNetwork =>
            loadInstallationsFromDb => replaceInstallationsInDb =>
                fetchInstallationsFromNetwork =>
                    location => loadInstallationsFromDb()
                        .Map(it => it.Map(it2 => it2.ToInstallation()))
                        .Map(it => it.ToList())
                        .Bind(it => IsLocationChanged(MaxInstallationDistance)(it, location)
                            ? fetchInstallationsFromNetwork(location).Bind(replaceInstallationsInDb)
                            : it);

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