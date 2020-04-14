using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using FirstLab.network;
using FirstLab.network.models;
using LaYumba.Functional;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Xamarin.Essentials;
using Index = FirstLab.network.models.Index;

namespace FirstLabUnitTests.network
{
    public class NetworkMeasurementsTests
    {
        [Test]
        public void ShouldRequestWithCorrectHeaders()
        {
            var baseUri = "http://example.com";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(baseUri + "*")
                .WithHeaders(new Dictionary<string, string>
                {
                    {"Accept", "application/json"},
                    {"apikey", "ExpectedApiKey"}
                })
                .Respond("application/json", Responses.MeasurementsResponse);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(baseUri);

            var networkUnderTest = new Network(client);

            var result = networkUnderTest.GetMeasurementsRequest2(8077);
            var value = GetValueFromEither(result);

            Assert.NotNull(value);
            mockHttp.VerifyNoOutstandingExpectation();
        }

        private static T GetValueFromEither<T>(Either<Error, T> either)
        {
            var option = new Option<T>();
            either.Match(error => Assert.Fail(error.Message), arg => option = arg);
            return option.GetOrElse(() => null).Result;
        }

        [Test]
        public void ShouldReturnDeserializedMeasurement()
        {
            var json = Responses.MeasurementsShorter;
            var result = Network.GetMeasurements(json);
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
        public void NameValueCollectionShouldContainsSpecifiedId()
        {
            var id = 1111;
            var result = Network.ByInstallationId(id);
            Assert.AreEqual(id.ToString(), result.Get("installationId"));
        }

        [Test]
        public void NameValueCollectionShouldContainSpecifiedLocationValues()
        {
            var location = new Location(52.2297, 21.0122);
            var result = Network.InstallationByLocation(location);
            Assert.AreEqual(location.Latitude.ToString(CultureInfo.InvariantCulture), result.Get("lat"));
            Assert.AreEqual(location.Longitude.ToString(CultureInfo.InvariantCulture), result.Get("lng"));
            Assert.AreEqual("-1", result.Get("maxDistanceKM"));
            Assert.AreEqual("1", result.Get("maxResults"));
        }

        [Test]
        public void ShouldReturnUriBuilderContainingCorrectBaseUrl()
        {
            var baseAddress = new Uri("https://example.com");
            var endpoint = "some/endpoint";
            var result = Network.CreateUriBuilder(baseAddress)(endpoint)(new NameValueCollection());
            Assert.IsTrue(result.Uri.ToString().StartsWith(baseAddress.ToString()));
        }

        [Test]
        public void ShouldReturnUriBuilderContainingCorrectPath()
        {
            var baseAddress = new Uri("https://example.com");
            var endpoint = "some/endpoint";
            var result = Network.CreateUriBuilder(baseAddress)(endpoint)(new NameValueCollection());
            Assert.AreEqual("/" + endpoint, result.Path);
        }
    }
}