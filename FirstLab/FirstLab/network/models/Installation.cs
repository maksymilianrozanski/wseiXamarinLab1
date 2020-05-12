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

        public bool Equals(Address other) => country == other.country && city == other.city && street == other.street;

        public override bool Equals(object obj) => obj is Address other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (country != null ? country.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (city != null ? city.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (street != null ? street.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Address left, Address right) => left.Equals(right);

        public static bool operator !=(Address left, Address right) => !left.Equals(right);

        public override string ToString() => $"{city}, {street}, {country}";
    }
}