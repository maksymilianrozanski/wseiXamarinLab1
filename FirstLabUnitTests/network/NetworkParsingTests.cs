using System.Collections.Generic;
using FirstLab.network;
using FirstLab.network.models;
using FirstLabUnitTests.utility;
using LaYumba.Functional;
using NUnit.Framework;
using Xamarin.Essentials;

namespace FirstLabUnitTests.network
{
    public class NetworkParsingTests
    {
        [Test]
        public void ShouldDeserializeMeasurement2()
        {
            var json = Responses.MeasurementsJsonResponseShorter;

            var result = TestUtilities.GetValueFromEither(Network.DeserializeMeasurements(json));
            var expected = new Measurements(new Current(
                "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
                new List<Value> {new Value("PM1", 13.61), new Value("PM25", 19.76)},
                new List<Index>
                {
                    new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
                        "Don't miss this day! The clean air calls!", "#D1CF1E")
                },
                new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}));
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldReturnFirstInstallationOfJson2()
        {
            var json = Responses.InstallationJsonResponse;
            var result = TestUtilities.GetValueFromEither(Network.DeserializeFirstInstallation(json));
            var expected = new Installation(8077, new Location(50.062006, 19.940984),
                new Address("Poland", "Krak├│w", "Miko┼éajska"));
            Assert.AreEqual(expected.id, result.id);
            Assert.AreEqual(expected.location.Latitude, result.location.Latitude);
            Assert.AreEqual(expected.location.Longitude, result.location.Longitude);
            Assert.AreEqual(expected.address, result.address);
        }

        [Test]
        public void ShouldReturnJsonParsingError()
        {
            //given
            var json = Responses.BrokenSensorJsonResponse;
            //when
            var result = Network.DeserializeJson<Measurements>(json);
            //then
            var errorOption = new Option<object>();
            result.Match(error => errorOption = error,
                measurements => Assert.Fail("Either should contain Left(error)."));
            var errorResult = errorOption.GetOrElse(() => null).Result;
            Assert.IsInstanceOf<JsonParsingError>(errorResult);
        }
    }
}