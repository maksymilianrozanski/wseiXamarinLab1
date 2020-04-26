using NUnit.Framework;
using SQLite;

namespace FirstLabUnitTests.db
{
    public class EntityTests
    {
        [Test]
        public void ShouldNotImpactNextTest()
        {
            var connection = new SQLiteConnection(":memory:");
            connection.CreateTable<TestEntity>();
            Assert.AreEqual(0, connection.Table<TestEntity>().Count(), "Table should be empty before inserting");
            connection.Insert(new TestEntity {SomeText = "Hello"});
            Assert.AreEqual(1, connection.Table<TestEntity>().Count(), "Table should contain one item after inserting");
        }

        [Test]
        public void ShouldNotImpactPreviousTest()
        {
            var connection = new SQLiteConnection(":memory:");
            connection.CreateTable<TestEntity>();
            Assert.AreEqual(0, connection.Table<TestEntity>().Count(), "Table should be empty before inserting");
            connection.Insert(new TestEntity {SomeText = "World"});
            Assert.AreEqual(1, connection.Table<TestEntity>().Count(), "Table should contain one item after insering");
        }
    }

    public class TestEntity
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }

        public string SomeText { get; set; }
    }
}