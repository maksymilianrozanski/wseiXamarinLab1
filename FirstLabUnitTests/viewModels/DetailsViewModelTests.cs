using System.Collections.Generic;
using System.Linq;
using FirstLab.network.models;
using FirstLab.viewModels;
using NUnit.Framework;
using Xamarin.Essentials;

namespace FirstLabUnitTests.viewModels
{
    public class DetailsViewModelTests
    {
        private static MeasurementVmItem CreateVmItem(double pm10 = 13.61, double pm25 = 19.76,
            double pressure = 1031.43,
            double humidity = 33.51, double temperature = 14.07, double indexValue = 37.52, string level = "LOW",
            string description = "Air is quite good.",
            string advice = "Don't miss this day! The clean air calls!", string color = "#D1CF1E"
        )
        {
            return new MeasurementVmItem
            {
                City = "Warsaw",
                Country = "Poland",
                Street = "Grove Street",
                Installation = new Installation(8077, new Location(50.062006, 19.940984),
                    new Address("Poland", "Warsaw", "Grove Street")),
                Measurements = new Measurements(new Current(
                    "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                    new List<Value>
                    {
                        new Value("PM10", pm10), new Value("PM25", pm25),
                        new Value("PRESSURE", pressure), new Value("HUMIDITY", humidity),
                        new Value("TEMPERATURE", temperature)
                    },
                    new List<Index>
                    {
                        new Index("AIRLY_CAQI", indexValue, level, description,
                            advice, color)
                    },
                    new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}))
            };
        }

        [Test]
        public void ShouldRoundValueToNearestInt_roundDown()
        {
            var input = 33.33;
            var expected = 33;
            var vmItem = CreateVmItem(humidity: input);
            var result = DetailsViewModel.ExtractIntValue("HUMIDITY")(vmItem);
            Assert.AreEqual(expected, result, "Should round value to nearest int");
        }

        [Test]
        public void ShouldRoundHumidityToNearestInt_roundUp()
        {
            var input = 33.51;
            var expected = 34;
            var vmItem = CreateVmItem(humidity: input);
            var result = DetailsViewModel.ExtractIntValue("HUMIDITY")(vmItem);
            Assert.AreEqual(expected, result, "Should round value to nearest int");
        }

        [Test]
        public void ShouldReturnZeroIfThereIsNoValueInVmItem()
        {
            var vmItem = CreateVmItem();
            var expected = -1;
            var result = DetailsViewModel.ExtractIntValue("NotPresent")(vmItem);
            Assert.AreEqual(expected, result, "Should return -1 if there is value with specified name in view model item");
        }
    }
}