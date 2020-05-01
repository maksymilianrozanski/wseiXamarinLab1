using System.Collections.Generic;
using System.Linq;
using FirstLab.db;
using FirstLab.entities;
using FirstLab.network.models;
using LaYumba.Functional;
using NUnit.Framework;
using SQLite;
using SQLiteNetExtensions.Extensions;
using Xamarin.Essentials;

namespace FirstLabUnitTests.db
{
    public class DbFunctionsTests
    {
        private readonly InstallationEntity _installationEntity1 =
            new Installation(10, new Location(50.2, 22.2), new Address("PL", "UnknownCity", "Str"))
                .ToInstallationEntity(
                    new Current(
                        "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                        new List<Value> {new Value("PM1", 13.61), new Value("PM25", 19.76)},
                        new List<Index>
                        {
                            new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                                "Don't miss this day! The clean air calls!", "#D1CF1E")
                        },
                        new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)})
                );

        private readonly Installation _installation2 =
            new Installation(11, new Location(51.2, 23.2), new Address("PL", "UnknownCity2", "Str"));

        private readonly Installation _installation3 =
            new Installation(12, new Location(51.2, 23.2), new Address("PL", "UnknownCity3", "Str"));

        private readonly InstallationEntity _installationEntity4 =
            new Installation(13, new Location(51.2, 22.2), new Address("PL", "UnknownCity4", "Str"))
                .ToInstallationEntity(
                    new Current(
                        "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                        new List<Value> {new Value("PM1", 13.61), new Value("PM25", 19.76)},
                        new List<Index>
                        {
                            new Index("AIRLY_CAQI", 90.0, "LOW", "Hello",
                                "Don't miss this day! The clean air calls!", "#D1CF1E")
                        },
                        new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)})
                );

        [Test]
        public void ShouldReplaceAllInstallationEntities()
        {
            var connection = new SQLiteConnection(":memory:");
            DatabaseHelper.CreateTables(connection);

            Assert.AreEqual(0, connection.Table<InstallationEntity>().Count(), "Table should be empty at start");
            connection.InsertWithChildren(_installationEntity1, true);

            var installations = new List<Installation> {_installation2, _installation3};

            DatabaseHelper.ReplaceInstallations(connection)(installations);
            Assert.AreEqual(2, connection.Table<InstallationEntity>().Count(),
                "Table should contain only 2 inserted items");
            Assert.AreEqual(0, connection.Table<ValueEntity>().Count(), "Should delete all children");
            Assert.AreEqual(0, connection.Table<StandardEntity>().Count(), "Should delete all children");
            Assert.AreEqual(0, connection.Table<IndexEntity>().Count(), "Should delete all children");
            Assert.AreEqual(0, connection.Table<CurrentEntity>().Count(), "Should delete all children");
        }

        [Test]
        public void ShouldReplaceCurrentEntity()
        {
            var connection = new SQLiteConnection(":memory:");
            DatabaseHelper.CreateTables(connection);
            connection.InsertWithChildren(_installationEntity1, true);

            var newMeasurements =
                (new Measurements(new Current(
                        "2020-05-08T07:31:50.230Z", "2020-05-08T08:31:50.230Z",
                        new List<Value> {new Value("PM10", 40.0), new Value("PM25", 19.76)},
                        new List<Index>
                        {
                            new Index("AIRLY_CAQI", 140.0, "HIGH", "Air is not good.",
                                "Don't miss this day! The clean air calls!", "#D1CF1E")
                        },
                        new List<Standard> {new Standard("WHO", "PM25", 25.0, 80.0)})),
                    new Installation(10, new Location(50.2, 22.2), new Address("PL", "UnknownCity", "Str")));

            var result = DatabaseHelper.ReplaceCurrent(connection)(newMeasurements);

            //verify
            result.Match(error => Assert.Fail("Should return result"), tuple =>
            {
                Assert.AreEqual(newMeasurements.Item1, tuple.Item1);
                Assert.AreEqual(newMeasurements.Item2, tuple.Item2);
            });

            var updatedEntity = connection.GetWithChildren<InstallationEntity>(10, true);
            var expectedCurrent = newMeasurements.Item1.current;
            var resultCurrentEntity = updatedEntity.CurrentEntity;
            Assert.AreEqual(expectedCurrent.fromDateTime, resultCurrentEntity.FromDateTime);
            Assert.AreEqual(expectedCurrent.tillDateTime, resultCurrentEntity.TillDateTime);
            Assert.AreEqual(expectedCurrent.values.First().name, resultCurrentEntity.Values.First().Name);
            Assert.AreEqual(expectedCurrent.values.First().value, resultCurrentEntity.Values.First().Value);
            Assert.AreEqual(expectedCurrent.values[1].name, resultCurrentEntity.Values[1].Name);
            Assert.AreEqual(expectedCurrent.values[1].value, resultCurrentEntity.Values[1].Value);
            Assert.AreEqual(expectedCurrent.indexes.First().value, resultCurrentEntity.IndexEntities.First().Value);
            Assert.AreEqual(expectedCurrent.indexes.First().description,
                resultCurrentEntity.IndexEntities.First().Description);
            Assert.AreEqual(expectedCurrent.standards.First().percent, resultCurrentEntity.Standards.First().Percent);
        }

        [Test]
        public void ShouldNotDeleteOtherCurrentEntityItems()
        {
            var connection = new SQLiteConnection(":memory:");
            DatabaseHelper.CreateTables(connection);
            connection.InsertWithChildren(_installationEntity1, true);

            var installationEntity2 =
                new Installation(11, new Location(50.2, 22.2), new Address("PL", "UnknownCity", "Str"))
                    .ToInstallationEntity(
                        new Current(
                            "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                            new List<Value> {new Value("PM1", 100.0), new Value("PM25", 19.76)},
                            new List<Index>
                            {
                                new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                                    "Don't miss this day! The clean air calls!", "#D1CF1E")
                            },
                            new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)})
                    );
            connection.InsertWithChildren(installationEntity2, true);

            var newMeasurements =
                (new Measurements(new Current(
                        "2020-05-08T07:31:50.230Z", "2020-05-08T08:31:50.230Z",
                        new List<Value> {new Value("PM10", 40.0), new Value("PM25", 19.76)},
                        new List<Index>
                        {
                            new Index("AIRLY_CAQI", 140.0, "HIGH", "Air is not good.",
                                "Don't miss this day! The clean air calls!", "#D1CF1E")
                        },
                        new List<Standard> {new Standard("WHO", "PM25", 25.0, 80.0)})),
                    new Installation(10, new Location(50.2, 22.2), new Address("PL", "UnknownCity", "Str")));

            var result = DatabaseHelper.ReplaceCurrent(connection)(newMeasurements);
            //verify
            result.Match(error => Assert.Fail("Should return result"), tuple =>
            {
                Assert.AreEqual(newMeasurements.Item1, tuple.Item1);
                Assert.AreEqual(newMeasurements.Item2, tuple.Item2);
            });

            var shouldNotBeModified = connection.GetWithChildren<InstallationEntity>(11, true);
            Assert.AreEqual(installationEntity2.CurrentEntity.Values.First().Value,
                shouldNotBeModified.CurrentEntity.Values.First().Value,
                "Values of remaining records should not be changed");

            Assert.AreEqual(2, connection.Table<CurrentEntity>().Count(),
                "Should delete previous values from database");
        }

        [Test]
        public void ShouldLoadInstallationEntitiesWithChildren()
        {
            var connection = new SQLiteConnection(":memory:");
            DatabaseHelper.CreateTables(connection);
            connection.InsertWithChildren(_installationEntity1, true);
            connection.InsertWithChildren(_installationEntity4, true);

            var expected = new List<InstallationEntity> {_installationEntity1, _installationEntity4};
            expected.Sort((i1, i2) => i1.Id.CompareTo(i2.Id));

            var result = DatabaseHelper.LoadInstallationEntities(connection)();
            //verify
            result.Match(error => Assert.Fail("Should return result"),
                list =>
                {
                    list.Sort((i1, i2) => i1.Id.CompareTo(i2.Id));
                    Assert.AreEqual(expected.First().Id, list.First().Id);
                    Assert.AreEqual(expected[1].Id, list[1].Id);
                    Assert.AreEqual(expected.First().CurrentEntity.Id, list.First().CurrentEntity.Id);
                    Assert.AreEqual(expected[1].CurrentEntity.Id, list[1].CurrentEntity.Id);
                    Assert.AreEqual(expected.First().CurrentEntity.IndexEntities.First().Description,
                        list.First().CurrentEntity.IndexEntities.First().Description);
                    Assert.AreEqual(expected[1].CurrentEntity.IndexEntities.First().Description,
                        list[1].CurrentEntity.IndexEntities.First().Description);
                }
            );
        }

        [Test]
        public void ShouldReturnCurrentEntity()
        {
            var connection = new SQLiteConnection(":memory:");
            DatabaseHelper.CreateTables(connection);

            connection.InsertWithChildren(_installationEntity1, true);

            var result = DatabaseHelper.LoadMeasurementByInstallationId(connection)(10);
            result.Match(error => Assert.Fail("Should match to the right"),
                option => option.Match(() => Assert.Fail("Should contain value"),
                    entity =>
                    {
                        Assert.AreEqual(_installationEntity1.Id, entity.InstallationId);
                        Assert.AreEqual(_installationEntity1.CurrentEntity.Id, entity.Id);
                        Assert.AreEqual(_installationEntity1.CurrentEntity.Values.First().Value,
                            entity.Values.First().Value, "Should fetch children recursively");
                    }));
        }

        [Test]
        public void ShouldReturnNone()
        {
            var connection = new SQLiteConnection(":memory:");
            DatabaseHelper.CreateTables(connection);

            connection.InsertWithChildren(_installationEntity1, true);
            var result = DatabaseHelper.LoadMeasurementByInstallationId(connection)(11);
            result.Match(error => Assert.Fail("Should match to the right"),
                option => option.Match(() => { }, entity => Assert.Fail("Should match to None"))
            );
        }
    }
}