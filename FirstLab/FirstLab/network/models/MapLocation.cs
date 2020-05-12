using Xamarin.Forms.Maps;

namespace FirstLab.network.models
{
    public readonly struct MapLocation
    {
        public string Address { get; }
        public string Description { get; }
        public Position Position { get; }

        public MapLocation(string address, string description, Position position)
        {
            Address = address;
            Description = description;
            Position = position;
        }

        public override string ToString() =>
            $"{Address}, {Description}, ({Position.Latitude}, {Position.Longitude})";
    }
}