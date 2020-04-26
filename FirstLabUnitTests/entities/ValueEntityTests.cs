using FirstLab.entities;
using FirstLab.network.models;
using NUnit.Framework;
using SQLite;

namespace FirstLabUnitTests.entities
{
    public class ValueEntityTests
    {
        [Test]
        public void ShouldBeAbleToSaveValueItem()
        {
            var connection = new SQLiteConnection(":memory:");
            var valueEntity = new Value("name", 12.0).ToValueEntity();
            connection.CreateTable<ValueEntity>();
            connection.Insert(valueEntity);
            Assert.AreEqual(1, connection.Table<ValueEntity>().Count());
        }

        [Test]
        public void ShouldBeAbleToRetrieveValueItem()
        {
            var connection = new SQLiteConnection(":memory:");
            var valueEntity = new Value("name", 12.0).ToValueEntity();
            connection.CreateTable<ValueEntity>();
            connection.Insert(valueEntity);

            var loadedItem = connection.Table<ValueEntity>().Take(1).First();
            Assert.AreEqual(valueEntity, loadedItem, "Saved and loaded item should be equal");
        }
    }
}