using System;
using Xamarin.Essentials;

namespace FirstLab.location
{
    public class LocationProvider
    {
        public static async void GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                Console.WriteLine("Will print location!");
                
                if (location != null)
                {
                    Console.WriteLine(
                        $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                throw;
                //     // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                throw;
                //     // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                throw;
                //     // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
                throw;
            }
        }
    }
}