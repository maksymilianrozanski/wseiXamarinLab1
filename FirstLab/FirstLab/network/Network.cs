using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using FirstLab.network.models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace FirstLab.network
{
    public class Network
    {
        private readonly HttpClient _client;

        private Network()
        {
        }

        public Network(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetNearestInstallationsRequest(Location location)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["lat"] = location.Latitude.ToString(CultureInfo.InvariantCulture);
            query["lng"] = location.Longitude.ToString(CultureInfo.InvariantCulture);
            query["maxDistanceKM"] = "-1";
            query["maxResults"] = "1";

            var uriBuilder = new UriBuilder(Path.Combine(_client.BaseAddress.ToString(), "v2/installations/nearest"))
            {
                Query = query.ToString()
            };
            var response = _client.GetAsync(uriBuilder.Uri.ToString()).Result;
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
            return null;
        }

        public static Installation GetNearestInstallation(string json)
        {
            return JsonConvert.DeserializeObject<List<Installation>>(json)[0];
        }

        public async Task<string> GetMeasurementsRequest(int id)
        {
            var uriBuilder = GetMeasurementsUri(_client.BaseAddress, id);
            var response = _client.GetAsync(uriBuilder.Uri).Result;
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
            return null;
        }

        public static UriBuilder GetMeasurementsUri(Uri baseAddress, int id)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["installationId"] = id.ToString();

            return
                new UriBuilder(Path.Combine(baseAddress.ToString(), "v2/measurements/installation/"))
                {
                    Query = query.ToString()
                };
        }

        public static Measurements GetMeasurements(string json)
        {
            return JsonConvert.DeserializeObject<Measurements>(json);
        }
    }
}