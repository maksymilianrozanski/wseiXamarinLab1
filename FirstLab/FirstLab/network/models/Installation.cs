using Xamarin.Essentials;

namespace FirstLab.network.models
{
    public readonly struct Installation
    {
        public readonly int id;
        public readonly Location location;
        public readonly Address address;
    }

    public readonly struct Address
    {
        public readonly string country;
        public readonly string city;
        public readonly string street;
    }
}