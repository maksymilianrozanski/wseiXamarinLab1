namespace FirstLab.network.models
{
    public readonly partial struct Index
    {
        public bool Equals(Index other) =>
            name == other.name && value.Equals(other.value) && level == other.level &&
            description == other.description && advice == other.advice && color == other.color;

        public override bool Equals(object obj) => obj is Index other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = name != null ? name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ value.GetHashCode();
                hashCode = (hashCode * 397) ^ (level != null ? level.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (description != null ? description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (advice != null ? advice.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (color != null ? color.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Index left, Index right) => left.Equals(right);
        public static bool operator !=(Index left, Index right) => !left.Equals(right);
    }
}