using System;
using System.Collections.Generic;
using System.Linq;
using FirstLab.entities;
using FirstLab.network.models;
using FirstLab.viewModels.home;
using LaYumba.Functional;
using Xamarin.Essentials;
using FetchVmItemsFunc =
    System.Func<System.Func<FirstLab.network.models.Installation, LaYumba.Functional.Either<LaYumba.Functional.Error, (
            FirstLab.network.models.Measurements, FirstLab.network.models.Installation)>>, System.Func<
            Xamarin.Essentials.Location, LaYumba.Functional.Either<LaYumba.Functional.Error,
                System.Collections.Generic.List<FirstLab.network.models.Installation>>>,
        Xamarin.Essentials.Location, LaYumba.Functional.Either<LaYumba.Functional.Error, (
            System.Collections.Generic.List<LaYumba.Functional.Error>,
            System.Collections.Generic.List<FirstLab.viewModels.home.MeasurementVmItem>)>>;
using FetchMeasurementsFromDbOrNetworkFunc =
    System.Func<System.Func<System.DateTime>,
        System.Func<int, LaYumba.Functional.Either<LaYumba.Functional.Error,
            LaYumba.Functional.Option<FirstLab.entities.CurrentEntity>>>,
        System.Func<(FirstLab.network.models.Measurements, FirstLab.network.models.Installation), LaYumba.Functional.
            Either<LaYumba.Functional.Error, (FirstLab.network.models.Measurements, FirstLab.network.models.Installation
                )>>, System.Func<FirstLab.network.models.Installation, LaYumba.Functional.Either<
            LaYumba.Functional.Error, (FirstLab.network.models.Measurements, FirstLab.network.models.Installation)>>,
        FirstLab.network.models.Installation, LaYumba.Functional.Either<LaYumba.Functional.Error, (
            FirstLab.network.models.Measurements, FirstLab.network.models.Installation)>>;
using FetchInstallationsFromDbOrNetworkFunc =
    System.Func<
        System.Func<LaYumba.Functional.Either<LaYumba.Functional.Error,
            System.Collections.Generic.List<FirstLab.entities.InstallationEntity>>>, System.Func<System.Func<
                System.Collections.Generic.List<FirstLab.network.models.Installation>, LaYumba.Functional.Either
                <LaYumba.Functional.Error, System.Collections.Generic.List<FirstLab.network.models.Installation>>>,
            System.
            Func<System.Func<Xamarin.Essentials.Location, LaYumba.Functional.Either<LaYumba.Functional.Error,
                System.Collections.Generic.List<FirstLab.network.models.Installation>>>, System.Func<
                Xamarin.Essentials.Location, LaYumba.Functional.Either<LaYumba.Functional.Error,
                    System.Collections.Generic.List<FirstLab.network.models.Installation>>>>>>;

namespace FirstLab.models.home
{
    internal static class HomeModel
    {
        internal static FetchVmItemsFunc FetchVmItems =>
            (measurementsOfInstallation, installationByLocation, currentLocation) =>
                installationByLocation(currentLocation)
                    .Map(it => it.Map(measurementsOfInstallation))
                    .Map(AggregateEithers)
                    .Match(error => (new List<Error>
                    {
                        error
                    }, new List<MeasurementVmItem>()), tuple =>
                        (tuple.Item1, HomeViewModel.MeasurementsInstallationToVmItem(tuple.Item2)));

        internal static FetchMeasurementsFromDbOrNetworkFunc FetchMeasurementsFromDbOrNetwork =>
            (dateFunc, measurementFromDbByInstallationId, replaceMeasurementInDb, fetchMeasurementsOfInstallation,
                    installation) =>
                measurementFromDbByInstallationId(installation.id)
                    .Bind(it =>
                        it.Match(
                            None: () => fetchMeasurementsOfInstallation(installation),
                            Some: entity => IsMeasurementObsolete(dateFunc, entity.ToMeasurement())
                                ? fetchMeasurementsOfInstallation(installation)
                                    .Bind(replaceMeasurementInDb)
                                : (entity.ToMeasurement(), installation)));

        public static FetchInstallationsFromDbOrNetworkFunc FetchInstallationsFromDbOrNetwork =>
            loadInstallationsFromDb => replaceInstallationsInDb =>
                fetchInstallationsFromNetwork =>
                    location => loadInstallationsFromDb()
                        .Map(it => it.Map(it2 => it2.ToInstallation()).ToList())
                        .Bind(it => IsLocationChanged(HomeViewModel.MaxInstallationDistance)(it, location)
                            ? fetchInstallationsFromNetwork(location).Bind(replaceInstallationsInDb)
                            : it);

        internal static bool IsMeasurementObsolete(Func<DateTime> getTime, Measurements measurement)
        {
            if (!DateTime.TryParse(measurement.current.tillDateTime, out var tillDate)) return false;
            var difference = getTime().Subtract(tillDate).TotalMinutes;
            return difference >= 60;
        }

        internal static (List<Error>, List<TR> ) AggregateEithers<TR>(IEnumerable<Either<Error, TR>> list) =>
            list.Aggregate((new List<Error>(), new List<TR>()), (acc, either) =>
            {
                either.Match(error => acc.Item1.Add(error),
                    value => acc.Item2.Add(value));
                return acc;
            });

        /// <summary>
        /// returns true if none of installations is in maxDistance range (in kilometers)
        /// </summary>
        public static Func<double, Func<List<Installation>, Location, bool>> IsLocationChanged =>
            maxDistance => (installations, currentLocation) =>
                !installations.Exists(installation =>
                    installation.location.CalculateDistance(currentLocation, DistanceUnits.Kilometers) <= maxDistance);
    }
}