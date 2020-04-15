using System;
using System.Collections.Generic;
using System.Globalization;
using FirstLab.network;
using FirstLabUnitTests.utility;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Xamarin.Essentials;

namespace FirstLabUnitTests.network
{
    public class NetworkTests
    {
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
                .Respond("application/json", Responses.InstallationJsonResponse);

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
                .Respond("application/json", Responses.InstallationJsonResponse);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(expectedBase);

            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetNearestInstallationsRequest(new Location(50.062006, 19.940984));
            var value = TestUtilities.GetValueFromEither(result);
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
                .Respond("application/json", Responses.InstallationJsonResponse);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(baseUrl);

            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetNearestInstallationsRequest(location);
            var value = TestUtilities.GetValueFromEither(result);
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
                .Respond("application/json", Responses.InstallationJsonResponse);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(baseUrl);

            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetNearestInstallationsRequest(location);
            var value = TestUtilities.GetValueFromEither(result);
            Assert.NotNull(value);
            mockHttp.VerifyNoOutstandingExpectation();
        }

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
                .Respond("application/json", Responses.MeasurementsJsonResponse);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(baseUri);

            var networkUnderTest = new Network(client);

            var result = networkUnderTest.GetMeasurementsRequest(8077);
            var value = TestUtilities.GetValueFromEither(result);

            Assert.NotNull(value);
            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}