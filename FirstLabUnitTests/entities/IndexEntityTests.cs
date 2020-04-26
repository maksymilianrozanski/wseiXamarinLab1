using FirstLab.entities;
using FirstLab.network.models;
using NUnit.Framework;
using SQLite;

namespace FirstLabUnitTests.entities
{
    public class IndexEntityTests
    {
        [Test]
        public void ShouldBeAbleToSaveIndexItem()
        {
            var connection = new SQLiteConnection(":memory:");
            var indexEntity = new Index("indexName", 12.0, "level", "description",
                "advice", "color").ToIndexEntity();
            connection.CreateTable<IndexEntity>();
            connection.Insert(indexEntity);
            Assert.AreEqual(1, connection.Table<IndexEntity>().Count());
        }

        [Test]
        public void ShouldBeAbleToRetrieveSavedIndexItem()
        {
            var connection = new SQLiteConnection(":memory:");
            var indexEntity = new Index("indexName", 12.0, "level", "description",
                "advice", "color").ToIndexEntity();

            connection.CreateTable<IndexEntity>();
            connection.Insert(indexEntity);

            var loadedItem = connection.Table<IndexEntity>().Take(1).First();
            Assert.AreEqual(indexEntity, loadedItem, "Saved and loaded item should be equal");
        }
    }
}