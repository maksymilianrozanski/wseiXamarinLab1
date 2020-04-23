using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Web;
using FirstLab.network.models;
using LaYumba.Functional;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace FirstLab.network
{
    public class Network
    {
        private const string MeasurementEndPoint = "v2/measurements/installation";
        private const string NearestInstallationEndpoint = "v2/installations/nearest";

        public static Func<Uri, Func<string, Func<NameValueCollection, UriBuilder>>> CreateUriBuilder =
            baseAddress => endpoint => queryValues =>
                new UriBuilder(Path.Combine(baseAddress.ToString(), endpoint))
                {
                    Query = queryValues.ToString()
                };

        public static Func<int, NameValueCollection> ByInstallationId = id =>
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["installationId"] = id.ToString();
            return query;
        };

        private readonly HttpClient _client;

        private Network()
        {
        }

        public Network(HttpClient client)
        {
            _client = client;
        }

        public Func<int, Func<Location, Either<Error, List<Installation>>>> GetNearestInstallationsRequest2 =>
            installations => location =>
            {
                var uriBuilder =
                    CreateUriBuilder(_client.BaseAddress)(NearestInstallationEndpoint)(
                        NearestInstallationsQuery(location, installations));
                var response = _client.GetAsync(uriBuilder.Uri.ToString()).Result;
                return CheckResponseStatus(response)
                    .Bind(ReadMessageContent)
                    .Bind(DeserializeInstallations);
            };

        public static HttpClient CreateClient()
        {
            var httpClient = new HttpClient {BaseAddress = new Uri("https://airapi.airly.eu")};
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("apiKey", App.ApiKey);
            return httpClient;
        }

        public static NameValueCollection NearestInstallationsQuery(Location location, int installations)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["lat"] = location.Latitude.ToString(CultureInfo.InvariantCulture);
            query["lng"] = location.Longitude.ToString(CultureInfo.InvariantCulture);
            query["maxDistanceKM"] = "-1";
            query["maxResults"] = installations.ToString();
            return query;
        }

        public Either<Error, List<Installation>> GetNearestInstallationsRequest(Location location, int installations)
            => GetNearestInstallationsRequest2(installations)(location);

        public Either<Error, Measurements> GetMeasurementsRequest(int id)
        {
            var uriBuilder = CreateUriBuilder(_client.BaseAddress)(MeasurementEndPoint)(ByInstallationId(id));
            var response = _client.GetAsync(uriBuilder.Uri.ToString()).Result;
            return CheckResponseStatus(response)
                .Bind(ReadMessageContent)
                .Bind(DeserializeMeasurements);
        }

        private Either<Error, HttpResponseMessage> CheckResponseStatus(HttpResponseMessage response) =>
            response.IsSuccessStatusCode
                ? (Either<Error, HttpResponseMessage>) response
                : new InvalidResponseCodeError("Not successful response status code: " + response.StatusCode);

        private Either<Error, string> ReadMessageContent(HttpResponseMessage message) =>
            message.Content.ReadAsStringAsync().Result;

        public static Either<Error, Measurements> DeserializeMeasurements(string json) =>
            DeserializeJson<Measurements>(json);

        public static Either<Error, Installation> DeserializeFirstInstallation(string json)
            => DeserializeJson<List<Installation>>(json)
                .Bind<Error, List<Installation>, Installation>(it => it[0]);

        public static Either<Error, List<Installation>> DeserializeInstallations(string json) =>
            DeserializeJson<List<Installation>>(json);

        public static Either<Error, T> DeserializeJson<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonSerializationException e)
            {
                return new JsonParsingError("Exception during deserializing json, message: " + e.Message + "json: " +
                                            json + ".");
            }
        }
    }

    public sealed class InvalidResponseCodeError : Error
    {
        public InvalidResponseCodeError(string message)
        {
            Message = message;
        }

        public override string Message { get; }
    }

    public sealed class JsonParsingError : Error
    {
        public JsonParsingError(string message)
        {
            Message = message;
        }

        public override string Message { get; }
    }
}