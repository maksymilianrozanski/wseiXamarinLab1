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

        public static Func<Location, NameValueCollection> InstallationByLocation = location =>
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["lat"] = location.Latitude.ToString(CultureInfo.InvariantCulture);
            query["lng"] = location.Longitude.ToString(CultureInfo.InvariantCulture);
            query["maxDistanceKM"] = "-1";
            query["maxResults"] = "1";
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

        public Either<Error, Installation> GetNearestInstallationsRequest(Location location)
        {
            var uriBuilder =
                CreateUriBuilder(_client.BaseAddress)(NearestInstallationEndpoint)(
                    InstallationByLocation(location));
            var response = _client.GetAsync(uriBuilder.Uri.ToString()).Result;
            return CheckResponseStatus(response)
                .Bind(ReadMessageContent)
                .Bind(DeserializeInstallation);
        }

        private Either<Error, HttpResponseMessage> CheckResponseStatus(HttpResponseMessage response)
        {
            return response.IsSuccessStatusCode
                ? (Either<Error, HttpResponseMessage>) response
                : new InvalidResponseCodeError("Not successful response status code: " + response.StatusCode);
        }

        private Either<Error, Installation> DeserializeInstallation(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<Installation>>(json)[0];
            } //TODO: edit to more specific exception type
            catch (Exception e)
            {
                return new JsonParsingError("Exception during deserializing json, message: " + e.Message + "json: " +
                                            json + ".");
            }
        }

        private Either<Error, string> ReadMessageContent(HttpResponseMessage message)
        {
            return message.Content.ReadAsStringAsync().Result;
        }

        public static Installation GetNearestInstallation(string json)
        {
            return JsonConvert.DeserializeObject<List<Installation>>(json)[0];
        }

        public Either<Error, Measurements> GetMeasurementsRequest2(int id)
        {
            var uriBuilder = CreateUriBuilder(_client.BaseAddress)(MeasurementEndPoint)(ByInstallationId(id));
            var response = _client.GetAsync(uriBuilder.Uri.ToString()).Result;
            return CheckResponseStatus(response)
                .Bind(ReadMessageContent)
                .Bind(DeserializeMeasurements);
        }

        public static Either<Error, Measurements> DeserializeMeasurements(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Measurements>(json);
            }
            catch (Exception e)
            {
                return new JsonParsingError("Exception during deserializing json, message: " + e.Message + "json: " +
                                            json + ".");
            }
        }

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

        public static Measurements GetMeasurements(string json)
        {
            return JsonConvert.DeserializeObject<Measurements>(json);
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