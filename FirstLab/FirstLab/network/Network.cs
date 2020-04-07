using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FirstLab.network
{
    public class Network
    {
        private HttpClient _client;

        private Network()
        {
        }

        public Network(HttpClient client)
        {
            _client = client;
        }
     
        
        
        public async Task<Task<string>> GetNearestInstallations(Location location)
        {
           
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["lat"] = location.Latitude.ToString(CultureInfo.InvariantCulture);
                query["lng"] = location.Longitude.ToString(CultureInfo.InvariantCulture);
                query["maxDistanceKM"] = "-1";
                query["maxResults"] = "1";
                
                var uriBuilder = new UriBuilder(Path.Combine(_client.BaseAddress.ToString(), "v2/installations/nearest/"))
                {
                    Query = query.ToString(),
                };
                var response = await _client.GetAsync(uriBuilder.Uri.ToString());
                    return response.Content.ReadAsStringAsync();
        }
    }
}