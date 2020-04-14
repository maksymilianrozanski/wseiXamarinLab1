using System;
using System.Collections.Generic;
using System.Globalization;
using FirstLab.network;
using FirstLab.network.models;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Xamarin.Essentials;

namespace FirstLabUnitTests.network
{
    public class NetworkTests
    {
        private const string ExampleContent =
            "[{\"id\":8077,\"location\":{\"latitude\":50.062006,\"longitude\":19.940984},\"address\":{\"country\":\"Poland\",\"city\":\"Krak├│w\",\"street\":\"Miko┼éajska\",\"number\":\"4\",\"displayAddress1\":\"Krak├│w\",\"displayAddress2\":\"Miko┼éajska\"},\"elevation\":220.38,\"airly\":true,\"sponsor\":{\"id\":489,\"name\":\"Chatham Financial\",\"description\":\"Airly Sensor's sponsor\",\"logo\":\"https://cdn.airly.eu/logo/ChathamFinancial_1570109001008_473803190.jpg\",\"link\":\"https://crossweb.pl/job/chatham-financial/ \"}}]";

        [Test]
        public void ShouldReturnHttpResponse()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://airapi.airly.eu/v2/installations/nearest/*")
                .WithHeaders(new Dictionary<string, string>
                {
                    {"Accept", "application/json"},
                    {"apikey", "ExpectedApiKey"}
                })
                .Respond("application/json", ExampleContent);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");

            var response = client.GetAsync("https://airapi.airly.eu/v2/installations/nearest/1").Result;
            var json = response.Content.ReadAsStringAsync();

            mockHttp.VerifyNoOutstandingExpectation();

            Console.WriteLine(json);
        }

        [Test]
        public void ShouldRequestCorrectBaseUrl()
        {
            var expectedBase = "http://example.com";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(expectedBase + "/*")
                .Respond("application/json", ExampleContent);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(expectedBase);

            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetNearestInstallationsRequest2(new Location(50.062006, 19.940984));
            var value = NetworkMeasurementsTests.GetValueFromEither(result);
            Assert.NotNull(value);
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public void GetRequestShouldContainCorrectValues()
        {
            var baseUrl = "http://example.com";

            var location = new Location(50.062006, 19.940984);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(baseUrl + "*")
                .WithQueryString(new Dictionary<string, string>
                {
                    {"lat", location.Latitude.ToString(CultureInfo.InvariantCulture)},
                    {"lng", location.Longitude.ToString(CultureInfo.InvariantCulture)},
                    {"maxDistanceKM", "-1"},
                    {"maxResults", "1"}
                })
                .Respond("application/json", ExampleContent);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(baseUrl);

            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetNearestInstallationsRequest2(location);
            var value = NetworkMeasurementsTests.GetValueFromEither(result);
            Assert.NotNull(value);
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public void GetNearestInstallationsShouldRequestCorrectApiEndpoint()
        {
            var baseUrl = "http://example.com";

            var location = new Location(50.062006, 19.940984);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(baseUrl + "/v2/installations/nearest")
                .Respond("application/json", ExampleContent);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(baseUrl);

            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetNearestInstallationsRequest2(location);
            var value = NetworkMeasurementsTests.GetValueFromEither(result);
            Assert.NotNull(value);
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public void ShouldReturnFirstInstallationOfJson()
        {
            var json = ExampleContent;
            var result = Network.GetNearestInstallation(json);
            var expected = new Installation(8077, new Location(50.062006, 19.940984),
                new Address("Poland", "Krak├│w", "Miko┼éajska"));
            Assert.AreEqual(expected.id, result.id);
            Assert.AreEqual(expected.location.Latitude, result.location.Latitude);
            Assert.AreEqual(expected.location.Longitude, result.location.Longitude);
            Assert.AreEqual(expected.address, result.address);
        }
    }
}