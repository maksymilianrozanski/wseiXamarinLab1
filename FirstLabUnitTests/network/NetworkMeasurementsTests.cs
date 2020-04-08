using System;
using System.Collections.Generic;
using FirstLab.network;
using NUnit.Framework;
using RichardSzalay.MockHttp;

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

            var result = networkUnderTest.GetMeasurementsRequest(8077).Result
                .Result;
            mockHttp.VerifyNoOutstandingExpectation();
        }


        [Test]
        public void ShouldRequestCorrectBaseUrl()
        {
            var expectedBase = "http://example.com";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(expectedBase + "*")
                .Respond("application/json", Responses.MeasurementsResponse);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(expectedBase);

            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetMeasurementsRequest(8077).Result
                .Result;
            mockHttp.VerifyNoOutstandingExpectation();
        }


        [Test]
        public void GetRequestShouldContainCorrectValues()
        {
            var baseUrl = "http://example.com";

            var expectedId = 8077;

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(baseUrl + "*")
                .WithQueryString(new Dictionary<string, string>
                {
                    {"id", expectedId.ToString()},
                })
                .Respond("application/json", Responses.MeasurementsResponse);

            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(baseUrl);

            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetMeasurementsRequest(expectedId).Result
                .Result;
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public void GetMeasurementsShouldRequestCorrectApiEndpoint()
        {
            var baseUrl = "http://example.com";
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(baseUrl + "/v2/measurements/installation/")
                .Respond("application/json", Responses.MeasurementsResponse);
            
            var client = mockHttp.ToHttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
            client.BaseAddress = new Uri(baseUrl);
            
            var networkUnderTest = new Network(client);
            var result = networkUnderTest.GetMeasurementsRequest(8077).Result
                .Result;
            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}