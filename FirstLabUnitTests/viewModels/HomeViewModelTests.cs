using System;
using System.Collections.Generic;
using FirstLab.network.models;
using FirstLab.viewModels;
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
            var value1 = new Value("PM1", 13.61);
            var value2 = new Value("PM25", 19.76);
            var value3 = new Value("PM10", 29.29);
            var value4 = new Value("PM25", 30.30);
            var address1 = new Address("Poland", "Krak├│w", "Miko┼éajska");
            var address2 = new Address("Poland", "Warszawa", "Some random street");

            var measurements1 = new Measurements(new Current(
                "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                new List<Value> {value1, value2},
                new List<Index>
                {
                    new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                        "Don't miss this day! The clean air calls!", "#D1CF1E")
                },
                new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}));

            var measurements2 = new Measurements(new Current(
                "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                new List<Value> {value3, value4},
                new List<Index>
                {
                    new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                        "Don't miss this day! The clean air calls!", "#D1CF1E")
                },
                new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}));


            var installation1 = new Installation(8077, new Location(50.062006, 19.940984),
                address1);


            var installation2 = new Installation(8077, new Location(59.062006, 29.940984),
                address2);

            var input = new List<(Measurements, Installation)>
            {
                (measurements1, installation1), (measurements2, installation2)
            };

            var result = HomeViewModel.MeasurementsInstallationToVmItem(input);

            var expected = new List<MeasurementVmItem>
            {
                new MeasurementVmItem
                {
                    City = address1.city, Country = address1.country, Street = address1.street, Name = value1.name,
                    Value = value1.value
                },
                new MeasurementVmItem
                {
                    City = address1.city, Country = address1.country, Street = address1.street, Name = value2.name,
                    Value = value2.value
                },
                new MeasurementVmItem
                {
                    City = address2.city, Country = address2.country, Street = address2.street, Name = value3.name,
                    Value = value3.value
                },
                new MeasurementVmItem
                {
                    City = address2.city, Country = address2.country, Street = address2.street, Name = value4.name,
                    Value = value4.value
                }
            };

            Assert.AreEqual(expected,result);
        }
    }
}