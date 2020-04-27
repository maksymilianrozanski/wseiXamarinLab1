using System.Collections.Generic;
using System.Linq;
using FirstLab.entities;
using FirstLab.network.models;
using LaYumba.Functional;
using NUnit.Framework;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace FirstLabUnitTests.entities
{
    public class CurrentEntityTests
    {
        [Test]
        public void ShouldBeAbleToSaveCurrentEntityItem()
        {
            var connection = new SQLiteConnection(":memory:");
            var currentEntity = new Current(
                "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                new List<Value> {new Value("PM1", 13.61), new Value("PM25", 19.76)},
                new List<Index>
                {
                    new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                        "Don't miss this day! The clean air calls!", "#D1CF1E")
                },
                new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}).ToCurrentEntity();

            connection.CreateTable<CurrentEntity>();
            connection.Insert(currentEntity);
            Assert.AreEqual(1, connection.Table<CurrentEntity>().Count());
        }

        [Test]
        public void ShouldBeAbleToRetrieveSavedCurrentEntityItem()
        {
            var connection = new SQLiteConnection(":memory:");

            connection.CreateTable<CurrentEntity>();
            connection.CreateTable<ValueEntity>();
            connection.CreateTable<StandardEntity>();
            connection.CreateTable<IndexEntity>();
            connection.CreateTable<InstallationEntity>();

            var currentEntity = new Current(
                "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                new List<Value> {new Value("PM1", 13.61), new Value("PM25", 19.76)},
                new List<Index>
                {
                    new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                        "Don't miss this day! The clean air calls!", "#D1CF1E")
                },
                new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}).ToCurrentEntity();


            connection.InsertWithChildren(currentEntity);

            var loadedItem = connection.Table<CurrentEntity>().Take(1).First()
                .Pipe(it => connection.GetWithChildren<CurrentEntity>(it.Id));

            Assert.True(CompareEntities(currentEntity, loadedItem), "Saved and loaded item should be equal");
        }

        private static bool CompareEntities(CurrentEntity first, CurrentEntity second) =>
            first.Id == second.Id && first.FromDateTime == second.FromDateTime &&
            ListsEqual(first.IndexEntities, second.IndexEntities)
            && ListsEqual(first.Values, second.Values) && ListsEqual(first.Standards, second.Standards);

        private static bool ListsEqual<T>(List<T> first, List<T> second) =>
            first.Count == second.Count &&
            first.Zip(second).All(tuple => tuple.First.Equals(tuple.Second));
    }
}