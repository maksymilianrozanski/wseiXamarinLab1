using System.Collections.Generic;
using System.Linq;
using FirstLab.entities;
using FirstLab.network.models;
using LaYumba.Functional;
using NUnit.Framework;
using SQLite;
using SQLiteNetExtensions.Extensions;
using Xamarin.Essentials;

namespace FirstLabUnitTests.entities
{
    public class InstallationEntityTests
    {
        private readonly InstallationEntity _installationEntity =
            new Installation(10, new Location(50.2, 22.2), new Address("PL", "UnknownCity", "Str"))
                .ToInstallationEntity(new List<Current>()
                {
                    new Current(
                        "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                        new List<Value> {new Value("PM1", 13.61), new Value("PM25", 19.76)},
                        new List<Index>
                        {
                            new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                                "Don't miss this day! The clean air calls!", "#D1CF1E")
                        },
                        new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)})
                });

        [Test]
        public void ShouldBeAbleToSaveInstallationEntityItem()
        {
            var connection = new SQLiteConnection(":memory:");

            connection.CreateTable<InstallationEntity>();
            connection.CreateTable<CurrentEntity>();
            connection.CreateTable<ValueEntity>();
            connection.CreateTable<StandardEntity>();
            connection.CreateTable<IndexEntity>();

            connection.Insert(_installationEntity);
            Assert.AreEqual(1, connection.Table<InstallationEntity>().Count());
        }

        [Test]
        public void ShouldBeAbleToRetrieveInstallationEntityItem()
        {
            var connection = new SQLiteConnection(":memory:");
            connection.CreateTable<InstallationEntity>();
            connection.CreateTable<CurrentEntity>();
            connection.CreateTable<ValueEntity>();
            connection.CreateTable<StandardEntity>();
            connection.CreateTable<IndexEntity>();

            connection.InsertWithChildren(_installationEntity, recursive: true);

            var result = connection.Table<InstallationEntity>().Take(1).First()
                .Pipe(it => connection.GetWithChildren<InstallationEntity>(it.Id, recursive: true));

            Assert.AreEqual(_installationEntity.Id, result.Id);
            Assert.AreEqual(_installationEntity.Address, result.Address);
            Assert.AreEqual(_installationEntity.Location, result.Location);
            Assert.AreEqual(_installationEntity.CurrentEntities.First().FromDateTime,
                result.CurrentEntities.First().FromDateTime);
            Assert.AreEqual(_installationEntity.CurrentEntities.First().TillDateTime,
                result.CurrentEntities.First().TillDateTime);
            Assert.AreEqual(_installationEntity.CurrentEntities.First().Id, result.CurrentEntities.First().Id);
            Assert.AreEqual(_installationEntity.CurrentEntities.First().Values.First(),
                result.CurrentEntities.First().Values.First());
            Assert.AreEqual(_installationEntity.CurrentEntities.First().Values[1],
                result.CurrentEntities.First().Values[1]);
            Assert.AreEqual(_installationEntity.CurrentEntities.First().Standards.First(),
                result.CurrentEntities.First().Standards.First());
            Assert.AreEqual(_installationEntity.CurrentEntities.First().IndexEntities.First(),
                result.CurrentEntities.First().IndexEntities.First());
        }
    }
}