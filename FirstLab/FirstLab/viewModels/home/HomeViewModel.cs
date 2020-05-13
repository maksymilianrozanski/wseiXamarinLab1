using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FirstLab.db;
using FirstLab.location;
using FirstLab.models.home;
using FirstLab.network;
using FirstLab.network.models;
using LaYumba.Functional;
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
using LoadItems =
    System.Func<Xamarin.Essentials.Location, LaYumba.Functional.Either<LaYumba.Functional.Error, (
        System.Collections.Generic.List<LaYumba.Functional.Error>,
        System.Collections.Generic.List<FirstLab.viewModels.home.MeasurementVmItem>)>>;

[assembly: InternalsVisibleTo("FirstLabUnitTests")]

namespace FirstLab.viewModels.home
{
    public partial class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(INavigation navigation) : base(navigation)
        {
            var httpClient = Network.CreateClient();
            _network = new Network(httpClient);

            MyCommand = new Command<MeasurementVmItem>(
                vmListItem => { navigation.PushAsync(new DetailsPage(vmListItem)); }
            );

            Task.Run(async () => await LoadMultipleValues(DefaultLoading));
        }

        private LoadItems ForceLoading => location =>
        {
            FetchMeasurementsOfInstallation measurementsFromNetwork = installation
                => FetchMeasurements(installation)
                    .Bind(DatabaseHelper.ReplaceCurrent2);

            FetchInstallationsByLocation
                installationsFromNetwork = (l) => FetchInstallations(l).Bind(DatabaseHelper.ReplaceInstallations2);

            return HomeModel.FetchVmItems(measurementsFromNetwork, installationsFromNetwork, location);
        };

        private LoadItems DefaultLoading => location =>
        {
            var measurementsFromDbOrNetwork =
                HomeModel.FetchMeasurementsFromDbOrNetwork.Apply(() => DateTime.Now)
                    .Apply(DatabaseHelper.LoadMeasurementByInstallationId2)
                    .Apply(DatabaseHelper.ReplaceCurrent2)
                    .Apply(FetchMeasurements);

            var installationsFromDbOrNetwork =
                HomeModel.FetchInstallationsFromDbOrNetwork
                        (DatabaseHelper.LoadInstallationEntities2)(DatabaseHelper.ReplaceInstallations2)
                    (FetchInstallations);

            return HomeModel.FetchVmItems(measurementsFromDbOrNetwork, installationsFromDbOrNetwork,
                location);
        };

        private async Task LoadMultipleValues(LoadItems fetchingFunc)
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
                MapLocations = CreateMapLocations(measurementVmItems);
                var numberOfErrors = errors.Count;
                ErrorMessage = numberOfErrors > 0 ? "Number of errors: " + numberOfErrors : "";
            });
        }

        private FetchInstallationsByLocation FetchInstallations =>
            _network.GetNearestInstallationsRequest2(2);

        private FetchMeasurementsOfInstallation FetchMeasurements => installation =>
            _network.GetMeasurementsRequest(installation.id)
                .Map(measurement => (measurement, installation));

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

        internal static List<MapLocation> CreateMapLocations(IEnumerable<MeasurementVmItem> vmItems) =>
            vmItems.Map(it => new MapLocation(it.Installation.address,
                "CAQI: " + it.Measurements.current.indexes.First().description,
                new Position(it.Installation.location.Latitude, it.Installation.location.Longitude))).ToList();
    }
}