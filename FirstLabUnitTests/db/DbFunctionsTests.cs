using System.Collections.Generic;
using System.Linq;
using FirstLab.db;
using FirstLab.entities;
using FirstLab.network.models;
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

        private readonly Installation _installationEntity2 =
            new Installation(11, new Location(51.2, 23.2), new Address("PL", "UnknownCity2", "Str"));

        private readonly Installation _installationEntity3 =
            new Installation(12, new Location(51.2, 23.2), new Address("PL", "UnknownCity3", "Str"));

        [Test]
        public void ShouldReplaceAllInstallationEntities()
        {
            var connection = new SQLiteConnection(":memory:");
            DatabaseHelper.CreateTables(connection);

            Assert.AreEqual(0, connection.Table<InstallationEntity>().Count(), "Table should be empty at start");
            connection.InsertWithChildren(_installationEntity1, true);

            var installations = new List<Installation> {_installationEntity2, _installationEntity3};

            DatabaseHelper.ReplaceInstallations(connection)(installations);
            Assert.AreEqual(2, connection.Table<InstallationEntity>().Count(),
                "Table should contain only 2 inserted items");
            Assert.AreEqual(0, connection.Table<ValueEntity>().Count(), "Should delete all children");
            Assert.AreEqual(0, connection.Table<StandardEntity>().Count(), "Should delete all children");
            Assert.AreEqual(0, connection.Table<IndexEntity>().Count(), "Should delete all children");
        }

        [Test]
        public void ShouldReplaceAllMeasurementEntities()
        {
            var connection = new SQLiteConnection(":memory:");
            DatabaseHelper.CreateTables(connection);
            connection.InsertWithChildren(_installationEntity1, true);

            var newMeasurements = new List<(Measurements, Installation)>
            {
                (new Measurements(new Current(
                        "2020-05-08T07:31:50.230Z", "2020-05-08T08:31:50.230Z",
                        new List<Value> {new Value("PM10", 40.0), new Value("PM25", 19.76)},
                        new List<Index>
                        {
                            new Index("AIRLY_CAQI", 140.0, "HIGH", "Air is not good.",
                                "Don't miss this day! The clean air calls!", "#D1CF1E")
                        },
                        new List<Standard> {new Standard("WHO", "PM25", 25.0, 80.0)})),
                    new Installation(10, new Location(50.2, 22.2), new Address("PL", "UnknownCity", "Str")))
            };

            var result = DatabaseHelper.ReplaceCurrents(connection)(newMeasurements);

            //verify
            result.Match(error => Assert.Fail("Should return result"), list =>
            {
                Assert.AreEqual(newMeasurements.First().Item1, list.First().Item1);
                Assert.AreEqual(newMeasurements.First().Item2, list.First().Item2);
                Assert.AreEqual(1, list.Count);
            });

            var updatedEntity = connection.GetWithChildren<InstallationEntity>(10, true);
            var expectedCurrent = newMeasurements.First().Item1.current;
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
    }
}