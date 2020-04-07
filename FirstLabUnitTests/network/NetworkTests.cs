using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FirstLab.network;
using Moq;
using Moq.Protected;
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
        public void ShouldRequestWithCorrectHeaders()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("*")
                .WithHeaders(new Dictionary<string, string>
                {
                    {"Accept", "application/json"},
                    {"apikey", "ExpectedApiKey"}
                })
                .Respond("application/json", ExampleContent);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");

            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetNearestInstallations(new Location(50.062006, 19.940984)).Result.Result;
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public void ShouldRequestCorrectBaseUrl()
        {
            var expectedBase = "http://example.com";
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(expectedBase + "*")
                .Respond("application/json", ExampleContent);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(expectedBase);
            
            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetNearestInstallations(new Location(50.062006, 19.940984)).Result.Result;
            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}