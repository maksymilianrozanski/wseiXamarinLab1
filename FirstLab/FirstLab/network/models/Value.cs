namespace FirstLab.network.models
{
    public readonly partial struct Value
    {
        public bool Equals(Value other) => name == other.name && value.Equals(other.value);
        public override bool Equals(object obj) => obj is Value other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0) * 397) ^ value.GetHashCode();
            }
        }

        public static bool operator ==(Value left, Value right) => left.Equals(right);
        public static bool operator !=(Value left, Value right) => !left.Equals(right);
    }
}