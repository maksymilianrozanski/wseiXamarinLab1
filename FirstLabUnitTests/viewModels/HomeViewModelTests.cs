using System;
using System.Collections.Generic;
using System.Linq;
using FirstLab.entities;
using FirstLab.network.models;
using FirstLab.viewModels;
using LaYumba.Functional;
using NUnit.Framework;
using Xamarin.Essentials;
using Index = FirstLab.network.models.Index;

namespace FirstLabUnitTests.viewModels
{
    public class HomeViewModelTests
    {
        [Test]
        public void ShouldConvertListToViewModelItems()
        {
            TestItems(out var installation1, out var installation2, out var measurements1, out var measurements2,
                out var address1, out var address2);

            var input = new List<(Measurements, Installation)>
            {
                (measurements1, installation1), (measurements2, installation2)
            };

            var result = HomeViewModel.MeasurementsInstallationToVmItem(input);

            var expected = new List<MeasurementVmItem>
            {
                new MeasurementVmItem
                {
                    City = address1.city, Country = address1.country, Street = address1.street,
                    Measurements = measurements1, Installation = installation1
                },
                new MeasurementVmItem
                {
                    City = address2.city, Country = address2.country, Street = address2.street,
                    Measurements = measurements2, Installation = installation2
                }
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldReturnErrorsListAndResultsList()
        {
            var input = new List<Either<Error, string>>
            {
                new TestError("test error message1"),
                "valid value",
                "valid value2",
                new TestError("test error message2")
            };

            var expectedErrors = new List<Error>
            {
                new TestError("test error message1"),
                new TestError("test error message2")
            };

            var expectedValues = new List<string> {"valid value", "valid value2"};

            var expected = (expectedErrors, expectedValues);

            var result = HomeViewModel.AggregateEithers(input);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldReturnVmItems()
        {
            var value1 = new Value("PM1", 13.61);
            var value2 = new Value("PM25", 19.76);

            var measurements1 = new Measurements(new Current(
                "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                new List<Value> {value1, value2},
                new List<Index>
                {
                    new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                        "Don't miss this day! The clean air calls!", "#D1CF1E")
                },
                new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}));
            var address1 = new Address("Poland", "Krak├│w", "Miko┼éajska");

            var installation1 = new Installation(8077, new Location(50.062006, 19.940984),
                address1);

            HomeViewModel.FetchVmItems(mi => mi)(l => l)
                (i => (measurements1, installation1))(location => new List<Installation> {installation1})
                (new Location(50.062006, 19.940984))
                .Match(error => Assert.Fail("Should not return error"), tuple =>
                {
                    var (errors, measurementVmItems) = tuple;
                    //then
                    Assert.IsEmpty(errors);
                    Assert.AreEqual(1, measurementVmItems.Count);
                    Assert.AreEqual(installation1, measurementVmItems.First().Installation);
                    Assert.AreEqual(measurements1, measurementVmItems.First().Measurements);
                    Assert.AreEqual(address1.city, measurementVmItems.First().City);
                    Assert.AreEqual(address1.country, measurementVmItems.First().Country);
                    Assert.AreEqual(address1.street, measurementVmItems.First().Street);
                });
        }

        [Test]
        public void ErrorWhenFetchingInstallations()
        {
            var testError = new TestError("Error when fetching installations");

            HomeViewModel.FetchVmItems(mi => mi)(l => l)
                (i => throw new Exception("Should not call this function"))(l => testError)
                (new Location(50.062006, 19.940984)).Match(
                    error => Assert.Fail("Should match Right"),
                    tuple =>
                    {
                        var (errors, measurementVmItems) = tuple;
                        Assert.AreEqual(testError, errors.First());
                        Assert.AreEqual(1, errors.Count);
                        Assert.IsEmpty(measurementVmItems);
                    }
                );
        }

        [Test]
        public void ErrorWhenFetchingOneOfMeasurements()
        {
            TestItems(out var installation1, out var installation2, out var measurements1, out _,
                out _, out _);

            var testError = new TestError("Fetching not successful...");

            Either<Error, (Measurements, Installation)> ErrorIfSecond(Installation i)
            {
                if (i.id == installation2.id) return testError;
                else return (measurements1, installation1);
            }

            var installations = new List<Installation> {installation1, installation2};

            HomeViewModel.FetchVmItems(mi => mi)(l => l)
                (ErrorIfSecond)(location => installations)
                (new Location(59.062006, 29.940984))
                .Match(
                    error => Assert.Fail("Should match Right"),
                    tuple =>
                    {
                        var (errors, measurementVmItems) = tuple;
                        Assert.AreEqual(1, errors.Count);
                        Assert.AreEqual(testError, errors.First());
                        Assert.AreEqual(1, measurementVmItems.Count,
                            "Should not return installation if it's measurements are not fetched.");
                        Assert.AreEqual(installation1, measurementVmItems.First().Installation,
                            "Should return installation whose measurements were fetched successfully");
                        Assert.AreEqual(measurements1, measurementVmItems.First().Measurements,
                            "Should contain measurements returned by measurementById function");
                    });
        }

        [Test]
        public void ShouldReturnItemFromDatabaseFunction()
        {
            TestItems(out var installation1, out _, out var measurementFromDb, out _,
                out _, out _);

            var functionUnderTest = HomeViewModel.FetchMeasurementsFromDbOrNetwork(() =>
                    DateTime.Parse(measurementFromDb.current.tillDateTime).AddMinutes(20))(i =>
                    (Option<CurrentEntity>) measurementFromDb.current.ToCurrentEntity())
                (i => throw new Exception("Should not fetch from network"));

            var result = functionUnderTest(installation1);

            result.Match(error => Assert.Fail("Should match to the right"), tuple =>
            {
                var (measurements, installation) = tuple;
                Assert.AreEqual(measurementFromDb, measurements);
                Assert.AreEqual(installation1, installation);
            });
        }

        [Test]
        public void ShouldReturnItemFromNetworkFunc_NoItemInDb()
        {
            TestItems(out var installation1, out _, out var measurementFromNetwork, out _,
                out _, out _);

            var functionUnderTest = HomeViewModel.FetchMeasurementsFromDbOrNetwork(
                    () => throw new Exception("Should not call this function"))
                (i => (Option<CurrentEntity>) null)
                (i => measurementFromNetwork);

            var result = functionUnderTest(installation1);

            result.Match(error => Assert.Fail("Should match to the right"), tuple =>
            {
                var (measurements, installation) = tuple;
                Assert.AreEqual(measurementFromNetwork, measurements);
                Assert.AreEqual(installation1, installation);
            });
        }

        [Test]
        public void ShouldReturnItemFromNetworkFunc_ObsoleteMeasurement()
        {
            TestItems(out var installation1, out _, out var obsoleteMeasurementFromDb, out var measurementFromNetwork,
                out _, out _);

            var functionUnderTest = HomeViewModel.FetchMeasurementsFromDbOrNetwork(() =>
                    DateTime.Parse(obsoleteMeasurementFromDb.current.tillDateTime).AddMinutes(70))(i =>
                    (Option<CurrentEntity>) obsoleteMeasurementFromDb.current.ToCurrentEntity())
                (i => measurementFromNetwork);

            var result = functionUnderTest(installation1);

            result.Match(error => Assert.Fail("Should match to the right"), tuple =>
            {
                var (measurements, installation) = tuple;
                Assert.AreEqual(measurementFromNetwork, measurements,
                    "Should return item from network function (not form database)");
                Assert.AreEqual(installation1, installation);
            });
        }

        private static void TestItems(out Installation installation1, out Installation installation2,
            out Measurements measurements1, out Measurements measurements2, out Address address1,
            out Address address2)
        {
            var value1 = new Value("PM1", 13.61);
            var value2 = new Value("PM25", 19.76);
            var value3 = new Value("PM10", 29.29);
            var value4 = new Value("PM25", 30.30);
            address1 = new Address("Poland", "Krak├│w", "Miko┼éajska");
            address2 = new Address("Poland", "Warszawa", "Some random street");

            measurements1 = new Measurements(new Current(
                "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                new List<Value> {value1, value2},
                new List<Index>
                {
                    new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                        "Don't miss this day! The clean air calls!", "#D1CF1E")
                },
                new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}));

            measurements2 = new Measurements(new Current(
                "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                new List<Value> {value3, value4},
                new List<Index>
                {
                    new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                        "Don't miss this day! The clean air calls!", "#D1CF1E")
                },
                new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}));

            installation1 = new Installation(8077, new Location(50.062006, 19.940984),
                address1);

            installation2 = new Installation(8078, new Location(59.062006, 29.940984),
                address2);
        }

        [Test]
        public void ShouldReturnTrueForRecentMeasurement()
        {
            TestItems(out _, out _, out var measurements1, out _,
                out _, out _);
            DateTime TimeFunc() => DateTime.Parse(measurements1.current.tillDateTime).AddMinutes(50);

            var result = HomeViewModel.IsMeasurementObsolete(TimeFunc, measurements1);
            Assert.IsFalse(result, "Should return false - difference less than 1h");
        }

        [Test]
        public void ShouldReturnFalseForObsoleteMeasurement()
        {
            TestItems(out _, out _, out var measurements1, out _,
                out _, out _);
            DateTime TimeFunc() => DateTime.Parse(measurements1.current.tillDateTime).AddMinutes(70);

            var result = HomeViewModel.IsMeasurementObsolete(TimeFunc, measurements1);
            Assert.IsTrue(result, "Should return true - difference more than 1h");
        }

        [Test]
        public void ShouldReturnTrue_LocationChanged()
        {
            TestItems(out var installation1, out var installation2, out _, out _,
                out _, out _);

            var maxDistance = 50.0;
            var installations = new List<Installation> {installation1, installation2};
            //1 deg. Latitude is about 111km
            var testLocation = new Location(installation2.location.Latitude + 1.0, installation2.location.Longitude);

            var result = HomeViewModel.IsLocationChanged(maxDistance)(installations, testLocation);
            Assert.IsTrue(result,
                $"Location changed - test location is farther than ${maxDistance} km from both installations");
        }

        [Test]
        public void ShouldReturnFalse_LocationNotChanged()
        {
            TestItems(out var installation1, out var installation2, out _, out _,
                out _, out _);

            var maxDistance = 50.0;
            var installations = new List<Installation> {installation1, installation2};
            //1 deg. Latitude is about 111km
            var testLocation = new Location(installation2.location.Latitude + 0.001, installation2.location.Longitude);

            var result = HomeViewModel.IsLocationChanged(maxDistance)(installations, testLocation);
            Assert.IsFalse(result,
                $"Location not changed - test location is closer than ${maxDistance} km from installation2");
        }

        [Test]
        public void ShouldFetchInstallationsFromNetworkFunc_LocationChanged()
        {
            TestItems(out var installation1, out var installation2, out _, out _,
                out _, out _);

            Either<Error, List<InstallationEntity>> InstallationsFromDb() => new List<InstallationEntity>
                {installation1.ToInstallationEntity(), installation2.ToInstallationEntity()};

            var replacedInstallationsCounter = 0;

            Either<Error, List<Installation>> ReplacingInstallationsFunc(List<Installation> list)
            {
                replacedInstallationsCounter += 1;
                return list;
            }

            Either<Error, List<Installation>> FetchingFromNetworkFunc(Location location) =>
                new List<Installation> {installation2};

            var functionUnderTest =
                HomeViewModel.FetchInstallationsFromDbOrNetwork(InstallationsFromDb)(ReplacingInstallationsFunc)(
                    FetchingFromNetworkFunc);

            var result = functionUnderTest(new Location(-80.0, -80.0));

            Assert.AreEqual(1, replacedInstallationsCounter, "Should try to replace installations in database once");
            result.Match(error => Assert.Fail("Should match to the right"), list =>
            {
                Assert.AreEqual(1, list.Count,
                    "Should return list from FetchingFromNetworkFunc, which contains 1 item");
                Assert.AreEqual(installation2, list.First(),
                    "Should return installation from FetchingFromNetworkFunc");
            });
        }

        [Test]
        public void ShouldReturnInstallationsFromDatabaseFunc_LocationNotChanged()
        {
            TestItems(out var installation1, out var installation2, out _, out _,
                out _, out _);

            Either<Error, List<InstallationEntity>> InstallationsFromDb() => new List<InstallationEntity>
                {installation1.ToInstallationEntity(), installation2.ToInstallationEntity()};

            var replacedInstallationsCounter = 0;

            Either<Error, List<Installation>> ReplacingInstallationsFunc(List<Installation> list)
            {
                replacedInstallationsCounter += 1;
                return list;
            }

            static Either<Error, List<Installation>> FetchingFromNetworkFunc(Location location) =>
                throw new Exception("Should not fetch from network function - location not changed");

            var functionUnderTest =
                HomeViewModel.FetchInstallationsFromDbOrNetwork(InstallationsFromDb)(ReplacingInstallationsFunc)(
                    FetchingFromNetworkFunc);

            var result = functionUnderTest(installation1.location);
            Assert.AreEqual(0, replacedInstallationsCounter,
                "Should not replace installations when location not changed");
            result.Match(error => Assert.Fail("Should match to the right"), list =>
            {
                Assert.AreEqual(2, list.Count(), "Should return list from InstallationsFromDb, which contains 2 items");
                Assert.AreEqual(installation1.id, list[0].id);
                Assert.AreEqual(installation2.id, list[1].id);
                Assert.AreEqual(installation1.address, list[0].address);
                Assert.AreEqual(installation2.address, list[1].address);
                Assert.AreEqual(installation1.location.Latitude, list[0].location.Latitude, 0.0001);
                Assert.AreEqual(installation1.location.Longitude, list[0].location.Longitude, 0.0001);
                Assert.AreEqual(installation2.location.Latitude, list[1].location.Latitude, 0.0001);
                Assert.AreEqual(installation2.location.Longitude, list[1].location.Longitude, 0.0001);
            });
        }

        private sealed class TestError : Error
        {
            public TestError(string message)
            {
                Message = message;
            }

            public override string Message { get; }

            private bool Equals(TestError other) => Message == other.Message;

            public override bool Equals(object obj) =>
                ReferenceEquals(this, obj) || obj is TestError other && Equals(other);

            public override int GetHashCode() => Message != null ? Message.GetHashCode() : 0;

            public static bool operator ==(TestError left, TestError right) => Equals(left, right);

            public static bool operator !=(TestError left, TestError right) => !Equals(left, right);
        }
    }
}