using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FirstLab.location
{
    public class LocationProvider
    {
        public static async Task<Location> GetLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            Console.WriteLine("Will print location!");

            if (location != null)
            {
                Console.WriteLine(
                    $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                return location;
            }

            Console.WriteLine("Null location!");
            throw new Exception("Null location");
        }
    }
}