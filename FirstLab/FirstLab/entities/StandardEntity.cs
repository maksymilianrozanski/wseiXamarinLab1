using SQLite;

namespace FirstLab.entities
{
    public class StandardEntity
    {
        public string Name { get; set; }
        public string Pollutant { get; set; }
        public double Limit { get; set; }
        public double Percent { get; set; }
        [PrimaryKey, AutoIncrement] public int Id { get; set; }

        public StandardEntity(string name, string pollutant, double limit, double percent)
        {
            Name = name;
            Pollutant = pollutant;
            Limit = limit;
            Percent = percent;
        }

        public StandardEntity()
        {
        }

        protected bool Equals(StandardEntity other) => Name == other.Name && Pollutant == other.Pollutant &&
                                                       Limit.Equals(other.Limit) && Percent.Equals(other.Percent) &&
                                                       Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StandardEntity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Pollutant != null ? Pollutant.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Limit.GetHashCode();
                hashCode = (hashCode * 397) ^ Percent.GetHashCode();
                hashCode = (hashCode * 397) ^ Id;
                return hashCode;
            }
        }

        public static bool operator ==(StandardEntity left, StandardEntity right) => Equals(left, right);

        public static bool operator !=(StandardEntity left, StandardEntity right) => !Equals(left, right);
    }
}