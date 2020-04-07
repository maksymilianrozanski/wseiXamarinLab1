using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

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
            var response = await _client.GetAsync("/v2/installations/nearest/1");
            return response.Content.ReadAsStringAsync();
        }
    }
}