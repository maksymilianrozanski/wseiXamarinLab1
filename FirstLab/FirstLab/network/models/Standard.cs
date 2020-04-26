namespace FirstLab.network.models
{
    public readonly partial struct Standard
    {
        public bool Equals(Standard other) =>
            name == other.name && pollutant == other.pollutant && limit.Equals(other.limit) &&
            percent.Equals(other.percent);

        public override bool Equals(object obj) => obj is Standard other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = name != null ? name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (pollutant != null ? pollutant.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ limit.GetHashCode();
                hashCode = (hashCode * 397) ^ percent.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Standard left, Standard right) => left.Equals(right);
        public static bool operator !=(Standard left, Standard right) => !left.Equals(right);
    }
}