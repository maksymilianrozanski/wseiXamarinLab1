using FirstLab.entities;
using FirstLab.network.models;
using NUnit.Framework;
using SQLite;

namespace FirstLabUnitTests.entities
{
    public class StandardEntityTests
    {
        [Test]
        public void ShouldBeAbleToSaveStandardItem()
        {
            var connection = new SQLiteConnection(":memory:");
            var standardEntity = new Standard("WHO", "PM25", 25.0, 79.05).ToStandardEntity();
            connection.CreateTable<StandardEntity>();
            connection.Insert(standardEntity);
            Assert.AreEqual(1, connection.Table<StandardEntity>().Count());
        }

        [Test]
        public void ShouldBeAbleToRetrieveSavedIndexItem()
        {
            var connection = new SQLiteConnection(":memory:");
            var standardEntity = new Standard("WHO", "PM25", 25.0, 79.05).ToStandardEntity();
            connection.CreateTable<StandardEntity>();
            connection.Insert(standardEntity);

            var loadedItem = connection.Table<StandardEntity>().Take(1).First();
            Assert.AreEqual(standardEntity, loadedItem, "Saved and loaded item should be equal");
        }
    }
}