
using Xamarin.Essentials;

namespace FirstLab.network.models
{
    public readonly struct Installation
    {
        public readonly int id;
        public readonly Location location;
        public readonly Address address;

        public Installation(int id, Location location, Address address)
        {
            this.id = id;
            this.location = location;
            this.address = address;
        }
    }

    public readonly struct Address
    {
        public readonly string country;
        public readonly string city;
        public readonly string street;

        public Address(string country, string city, string street)
        {
            this.country = country;
            this.city = city;
            this.street = street;
        }
    }
}