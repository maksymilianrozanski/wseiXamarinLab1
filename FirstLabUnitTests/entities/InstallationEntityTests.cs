using FirstLab.entities;
using FirstLab.network.models;
using NUnit.Framework;
using SQLite;
using Xamarin.Essentials;

namespace FirstLabUnitTests.entities
{
    public class InstallationEntityTests
    {
        [Test]
        public void ShouldBeAbleToSaveInstallationEntityItem()
        {
            var connection = new SQLiteConnection(":memory:");
            var installationEntity =
                new Installation(10, new Location(50.2, 22.2), new Address("PL", "UnknownCity", "Str"))
                    .ToInstallationEntity();

            connection.CreateTable<InstallationEntity>();
            connection.Insert(installationEntity);
            Assert.AreEqual(1, connection.Table<InstallationEntity>().Count());
        }

        [Test]
        public void ShouldBeAbleToRetrieveInstallationEntityItem()
        {
            var connection = new SQLiteConnection(":memory:");
            var installationEntity =
                new Installation(10, new Location(50.2, 22.2), new Address("PL", "UnknownCity", "Str"))
                    .ToInstallationEntity();

            connection.CreateTable<InstallationEntity>();
            connection.Insert(installationEntity);

            var result = connection.Table<InstallationEntity>().Take(1).First();

            Assert.AreEqual(installationEntity.Id, result.Id);
            Assert.AreEqual(installationEntity.Address, result.Address);
            Assert.AreEqual(installationEntity.Location, result.Location);
        }
    }
}