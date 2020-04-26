using System.Linq;

namespace FirstLab.network.models
{
    public readonly partial struct Current
    {
        public bool Equals(Current other) =>
            fromDateTime == other.fromDateTime && tillDateTime == other.tillDateTime &&
            values.SequenceEqual(other.values) && indexes.SequenceEqual(other.indexes) &&
            standards.SequenceEqual(other.standards);

        public override bool Equals(object obj) => obj is Current other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = fromDateTime != null ? fromDateTime.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (tillDateTime != null ? tillDateTime.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (values != null ? values.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (indexes != null ? indexes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (standards != null ? standards.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Current left, Current right) => left.Equals(right);
        public static bool operator !=(Current left, Current right) => !left.Equals(right);
    }
}