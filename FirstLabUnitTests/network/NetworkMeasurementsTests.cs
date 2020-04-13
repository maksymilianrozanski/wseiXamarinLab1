// using System;
// using System.Collections.Generic;
// using System.Linq;
// using FirstLab.network;
// using FirstLab.network.models;
// using NUnit.Framework;
// using RichardSzalay.MockHttp;
// using Xamarin.Forms;
// using Index = FirstLab.network.models.Index;
//
// namespace FirstLabUnitTests.network
// {
//     public class NetworkMeasurementsTests
//     {
//         [Test]
//         public void ShouldRequestWithCorrectHeaders()
//         {
//             var baseUri = "http://example.com";
//             var mockHttp = new MockHttpMessageHandler();
//             mockHttp.When(baseUri + "*")
//                 .WithHeaders(new Dictionary<string, string>
//                 {
//                     {"Accept", "application/json"},
//                     {"apikey", "ExpectedApiKey"}
//                 })
//                 .Respond("application/json", Responses.MeasurementsResponse);
//
//             var client = mockHttp.ToHttpClient();
//             client.DefaultRequestHeaders.Add("Accept", "application/json");
//             client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
//             client.BaseAddress = new Uri(baseUri);
//
//             var networkUnderTest = new Network(client);
//
//             var result = networkUnderTest.GetMeasurementsRequest(8077).Result
//                 .Result;
//             mockHttp.VerifyNoOutstandingExpectation();
//         }
//
//
//         [Test]
//         public void ShouldRequestCorrectBaseUrl()
//         {
//             var expectedBase = "http://example.com";
//
//             var mockHttp = new MockHttpMessageHandler();
//             mockHttp.When(expectedBase + "*")
//                 .Respond("application/json", Responses.MeasurementsResponse);
//
//             var client = mockHttp.ToHttpClient();
//             client.DefaultRequestHeaders.Add("Accept", "application/json");
//             client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
//             client.BaseAddress = new Uri(expectedBase);
//
//             var networkUnderTest = new Network(client);
//             var result = networkUnderTest.GetMeasurementsRequest(8077).Result
//                 .Result;
//             mockHttp.VerifyNoOutstandingExpectation();
//         }
//
//
//         [Test]
//         public void GetRequestShouldContainCorrectValues()
//         {
//             var baseUrl = "http://example.com";
//
//             var expectedId = 8077;
//
//             var mockHttp = new MockHttpMessageHandler();
//             mockHttp.When(baseUrl + "*")
//                 .WithQueryString(new Dictionary<string, string>
//                 {
//                     {"id", expectedId.ToString()},
//                 })
//                 .Respond("application/json", Responses.MeasurementsResponse);
//
//             var client = mockHttp.ToHttpClient();
//             client.DefaultRequestHeaders.Add("Accept", "application/json");
//             client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
//             client.BaseAddress = new Uri(baseUrl);
//
//             var networkUnderTest = new Network(client);
//             var result = networkUnderTest.GetMeasurementsRequest(expectedId).Result
//                 .Result;
//             mockHttp.VerifyNoOutstandingExpectation();
//         }
//
//         [Test]
//         public void GetMeasurementsShouldRequestCorrectApiEndpoint()
//         {
//             var baseUrl = "http://example.com";
//
//             var mockHttp = new MockHttpMessageHandler();
//             mockHttp.When(baseUrl + "/v2/measurements/installation/")
//                 .Respond("application/json", Responses.MeasurementsResponse);
//
//             var client = mockHttp.ToHttpClient();
//             client.DefaultRequestHeaders.Add("Accept", "application/json");
//             client.DefaultRequestHeaders.Add("apikey", "ExpectedApiKey");
//             client.BaseAddress = new Uri(baseUrl);
//
//             var networkUnderTest = new Network(client);
//             var result = networkUnderTest.GetMeasurementsRequest(8077).Result
//                 .Result;
//             mockHttp.VerifyNoOutstandingExpectation();
//         }
//
//         [Test]
//         public void ShouldReturnDeserializedMeasurement()
//         {
//             var json = Responses.MeasurementsShorter;
//             var result = Network.GetMeasurements(json);
//             var expected = new Measurements(new Current(
//                 "2020-04-08T07:31:50.230Z", "2020-04-08T08:31:50.230Z",
//                 new List<Value> {new Value("PM1", 13.61), new Value("PM25", 19.76)},
//                 new List<Index>
//                 {
//                     new Index("AIRLY_CAQI", 37.52, "LOW", "Air is quite good.",
//                         "Don't miss this day! The clean air calls!", "#D1CF1E")
//                 },
//                 new List<Standard> {new Standard("WHO", "PM25", 25.0, 79.05)}));
//
//             Assert.AreEqual(expected, result);
//         }
//     }
// }

